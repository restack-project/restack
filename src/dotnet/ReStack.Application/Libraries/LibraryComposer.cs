using ReStack.Application.Libraries.Models;
using ReStack.Common.Factories;
using ReStack.Common.Helpers;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Domain.Entities;
using ReStack.Domain.ValueObjects;
using System.Text.Json;

namespace ReStack.Application.Libraries;

public class LibraryComposer : ILibraryComposer, IDisposable
{
    private readonly IComponentLibraryRepository _repository;
    private readonly List<string> _tempLocations = new();

    public LibraryComposer(IComponentLibraryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ComposeResult> Compose(string source)
    {
        var result = new ComposeResult();

        try
        {
            result.ComponentLibrary = await _repository.GetBySource(source);
            result.ComponentLibrary ??= new ComponentLibrary() { Source = source.Replace(".git", string.Empty) };
            result.GitDirectory = await CloneGit(result.ComponentLibrary.Source);

            await ValidateRepository(result);

            if (!result.Success)
                return result;

            await ComposeLibrary(result);

            return result;
        }
        catch (Exception ex)
        {
            result.AddValidation("General", $"Error while composing library: {ex.Message}");
        }

        return result;
    }

    private static Task ValidateRepository(ComposeResult result)
    {
        var manifestPath = Path.Combine(result.GitDirectory, "manifest.json");
        if (!File.Exists(manifestPath))
            result.AddValidation("General", $"No manifest.json file found. A manifest.json file is required on the top level of the git repository");

        var componentDirectory = Path.Combine(result.GitDirectory, "components");
        if (!Directory.Exists(componentDirectory))
            result.AddValidation("General", "No component directory found. A component directory is required on the top level of the git repository");

        return Task.CompletedTask;
    }

    private static async Task ComposeLibrary(ComposeResult result)
    {
        var manifestPath = Path.Combine(result.GitDirectory, "manifest.json");
        var json = File.ReadAllText(manifestPath);

        if (TrySerialize<Manifest>(json, result, "General", "Error while parsing manifest.json", out var manifest))
        {
            result.ComponentLibrary.Version = manifest.Version;
            result.ComponentLibrary.Name = manifest.Name;
            result.ComponentLibrary.CodeOwners = string.Join(",", manifest.CodeOwners ?? new());
            result.ComponentLibrary.Documentation = manifest.Documentation;
            result.ComponentLibrary.Slug = GitHelper.GetSlug(result.ComponentLibrary.Source);
            result.ComponentLibrary.LastHashCommit = await GetLastHashCommit(result.GitDirectory);

            if (Enum.TryParse<ProgrammingLanguage>(manifest.Type, ignoreCase: true, out var type))
                result.ComponentLibrary.Type = type;
            else
                result.AddValidation("General", $"The manifest.json contains an invalid 'Type' ({manifest.Type}), only following are allowed: '{string.Join(",", Enum.GetNames<ProgrammingLanguage>())}'");

            var componentDirectory = Path.Combine(result.GitDirectory, "components");
            foreach (var componentFolder in Directory.GetDirectories(componentDirectory))
            {
                await ComposeComponent(result, componentFolder);
            }
        }
    }

    private static async Task ComposeComponent(ComposeResult result, string componentFolder)
    {
        var metadataPath = Path.Combine(componentFolder, "metadata.json");

        if (!File.Exists(metadataPath))
        {
            result.AddValidation(Path.GetFileName(componentFolder), $"Component without metadata.json file is found.");
            return;
        }

        var json = File.ReadAllText(metadataPath);

        if (TrySerialize<Metadata>(json, result, Path.GetFileName(componentFolder), $"Error while parsing metadata", out var metadata))
        {
            var existingComponent = result.ComponentLibrary.Components.FirstOrDefault(x => x.Name.ToLowerInvariant() == metadata.Name.ToLowerInvariant());
            if (existingComponent is null)
            {
                existingComponent = new();
                result.ComponentLibrary.Components.Add(existingComponent);
            }

            existingComponent.Name = metadata.Name;
            existingComponent.FolderName = Path.GetFileName(componentFolder);
            existingComponent.ComponentLibraryId = result.ComponentLibrary.Id;
            existingComponent.FileName = metadata.File;

            if (!File.Exists(Path.Combine(componentFolder, metadata.File ?? string.Empty)))
                result.AddValidation(existingComponent.Name, $"Component defines file '{metadata.File}' but this does not exist in the component folder.");

            foreach (var parameter in metadata.Parameters)
            {
                var existingParameter = existingComponent.Parameters.FirstOrDefault(x => x.Name.ToLowerInvariant() == parameter.Name.ToLowerInvariant());
                if (existingParameter is null)
                {
                    existingParameter = new ComponentParameter();
                    existingComponent.Parameters.Add(existingParameter);
                }

                existingParameter.Name = parameter.Name;

                if (Enum.TryParse<DataType>(parameter.DataType, ignoreCase: true, out var dataType))
                    existingParameter.DataType = dataType;
                else
                    result.AddValidation(existingComponent.Name, $"Component has invalid parameter '{parameter.Name}' with datatype '{parameter.DataType}'. Only the following are supported '{string.Join(",", Enum.GetNames<DataType>())}'");
            }

            await Task.CompletedTask;
        }
    }

    private static async Task<string> GetLastHashCommit(string gitDirectory)
    {
        var process = ProcessFactory.CreateDefault("git", $"rev-parse HEAD");
        process.StartInfo.WorkingDirectory = gitDirectory;

        process.Start();

        var result = process.StandardOutput.ReadToEnd();

        await process.WaitForExitAsync();

        return result.Replace("\n", string.Empty);
    }

    private async Task<string> CloneGit(string url)
    {
        var tempDirectory = Directory.CreateTempSubdirectory();
        var process = ProcessFactory.CreateDefault("git", $"clone {url} \"{tempDirectory.FullName}\"");

        process.Start();

        process.StandardOutput.ReadToEnd();
        process.StandardError.ReadToEnd();

        await process.WaitForExitAsync();

        _tempLocations.Add(tempDirectory.FullName);

        return tempDirectory.FullName;
    }

    private static bool TrySerialize<TE>(string json, ComposeResult result, string errorKey, string errorValue, out TE parsed)
    {
        parsed = default;

        try
        {
            parsed = JsonSerializer.Deserialize<TE>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return true;
        }
        catch (Exception ex)
        {
            result.AddValidation(errorKey, $"{errorValue}: {json.Replace(Environment.NewLine, string.Empty)} with exception '{ex.Message}'");
        }

        return false;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "<Pending>")]
    public void Dispose()
    {
        try
        {
            foreach (var location in _tempLocations)
            {
                var gitFolder = Path.Combine(location, ".git", "objects", "pack");

                if (File.Exists(gitFolder))
                {
                    foreach (var file in Directory.GetFiles(gitFolder))
                        File.SetAttributes(file, FileAttributes.Normal);
                }

                if (Directory.Exists(location))
                    Directory.Delete(location, true);
            }
        }
        catch { }
    }
}
