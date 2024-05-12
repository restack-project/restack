using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Options;
using ReStack.Application.Aggregates.Base;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Aggregates;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Common.Models;
using ReStack.Domain.Entities;
using ReStack.Domain.Settings;

namespace ReStack.Application.Aggregates;

public class StackAggregate(
    IStackRepository _repository
    , IValidator<StackModel> _validator
    , IMapper _mapper
    , IStackExecutor _stackExecutor
    , IOptions<ApiSettings> _options
) : BaseAggregate<StackModel, Stack, IStackRepository>(_repository, _validator, _mapper), IStackAggregate
{
    private readonly ApiSettings _settings = _options.Value;

    public override async Task<StackModel> Add(StackModel model)
    {
        model = await base.Add(model);

        var entity = await _repository.Get(model.Id);

        Directory.CreateDirectory(Path.GetDirectoryName(entity.GetLocation(_settings)));
        using var _ = File.Create(entity.GetLocation(_settings));

        return model;
    }

    public override async Task<StackModel> Update(StackModel model)
    {
        var entity = await _repository.Get(model.Id);
        var extension = entity.GetExtension();

        model = await base.Update(model);

        entity = await _repository.Get(entity.Id);
        var newExtension = entity.GetExtension();

        if (newExtension != extension)
        {
            var filePath = entity.GetLocation(_settings);
            
            Path.ChangeExtension(filePath, newExtension);

            File.Delete(filePath);
        }

        return model;
    }

    public override async Task<StackModel> Delete(int id)
    {
        var entity = await _repository.Get(id);
        var model = await base.Delete(id);

        try
        {
            var directory = Path.GetDirectoryName(entity.GetLocation(_settings));

            if (Directory.Exists(directory))
            {
                Directory.Delete(Path.GetDirectoryName(entity.GetLocation(_settings)), recursive: true);
            }
        }
        catch (Exception)
        {
            // todo proper error handling
            throw;
        }

        return model;
    }

    public async Task<JobModel> Execute(int stackId)
    {
        var stackEntity = await _repository.Get(stackId);
        var jobEntity = await _stackExecutor.Queue(stackEntity);
        var jobModel = _mapper.Map<JobModel>(jobEntity);

        return jobModel;
    }

    public override Task<List<StackModel>> GetAll(CancellationToken cancellationToken = default)
        => GetAll(numberOfJobs: 1, cancellationToken);

    public async Task<List<StackModel>> GetAll(int numberOfJobs, CancellationToken token = default)
    {
        var entities = await _repository.GetAll(numberOfJobs, token);
        var models = _mapper.Map<List<StackModel>>(entities);

        return models;
    }

    public async Task<string> DownloadFile(int stackId, CancellationToken token = default)
    {
        var stack = await _repository.Get(stackId, token);
        var path = stack.GetLocation(_settings);
        var text = File.ReadAllText(path);

        return text;
    }

    public async Task<StackModel> UploadFile(int stackId, string text)
    {
        var stack = await _repository.Get(stackId);
        var stackFilePath = stack.GetLocation(_settings);

        if (!File.Exists(stackFilePath))
            File.WriteAllText(stackFilePath, string.Empty);

        if (OperatingSystem.IsLinux())
            text = text.Replace("\r\n", "\n");

        File.WriteAllText(stackFilePath, text);

        var model = _mapper.Map<StackModel>(stack);
        return model;
    }
}
