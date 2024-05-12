using Microsoft.Extensions.Options;
using ReStack.Common.Exceptions;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Domain.Entities;
using ReStack.Domain.Settings;

namespace ReStack.Application.Libraries;

public class LibrarySync : ILibrarySync
{
    private readonly ILibraryComposer _libraryComposer;
    private readonly IComponentLibraryRepository _repository;
    private readonly ApiSettings _settings;

    public LibrarySync(ILibraryComposer libraryComposer, IComponentLibraryRepository repository, IOptions<ApiSettings> options)
    {
        _libraryComposer = libraryComposer;
        _repository = repository;
        _settings = options.Value;
    }

    public async Task<ComponentLibrary> Sync(string source)
    {
        var composeResult = await _libraryComposer.Compose(source);

        if (!composeResult.Success)
            throw new LibraryComposeException(composeResult.Validations);

        if (composeResult.ComponentLibrary.Id == 0)
            composeResult.ComponentLibrary = await _repository.Add(composeResult.ComponentLibrary);
        else
            composeResult.ComponentLibrary = await _repository.Update(composeResult.ComponentLibrary);

        var libraryDirectory = composeResult.ComponentLibrary.GetLocation(_settings);

        Clear(libraryDirectory);
        CopyFromGit(composeResult.GitDirectory, libraryDirectory);

        return composeResult.ComponentLibrary;
    }

    private static void CopyFromGit(string gitDirectory, string libraryDirectory)
    {
        Directory.CreateDirectory(libraryDirectory);

        foreach (var sourcePath in Directory.GetFiles(gitDirectory, "*", SearchOption.AllDirectories))
        {
            if (!sourcePath.Contains(".git"))
            {
                var relativePath = Path.GetRelativePath(gitDirectory, sourcePath);
                var targetPath = Path.Combine(libraryDirectory, relativePath);

                var file = new FileInfo(sourcePath);
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                file.MoveTo(targetPath);

                if (!OperatingSystem.IsWindows())
                    File.SetUnixFileMode(targetPath, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
            }
        }
    }

    private static void Clear(string path)
    {
        Directory.CreateDirectory(path);

        foreach (var file in Directory.GetFiles(path))
            File.SetAttributes(file, FileAttributes.Normal);

        Directory.Delete(path, recursive: true);
    }
}
