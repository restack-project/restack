using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReStack.Application.Aggregates.Base;
using ReStack.Common.Exceptions;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Aggregates;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Common.Models;
using ReStack.Domain.Entities;
using ReStack.Domain.Settings;

namespace ReStack.Application.Aggregates;

public class ComponentLibraryAggregate(
    IComponentLibraryRepository _repository
    , IValidator<ComponentLibraryModel> _validator
    , IMapper _mapper
    , ILibrarySync _librarySync
    , ILibraryComposer _libraryComposer
    , IOptions<ApiSettings> _options
    , ILogger<ComponentLibraryAggregate> _logger
) : BaseAggregate<ComponentLibraryModel, ComponentLibrary, IComponentLibraryRepository>(_repository, _validator, _mapper), IComponentLibraryAggregate
{
    private readonly ApiSettings _settings = _options.Value;

    public async Task<List<StackModel>> GetUsingStacks(int componentLibraryId)
    {
        var entities = await _repository.GetUsingStacks(componentLibraryId);
        var model = _mapper.Map<List<StackModel>>(entities);

        return model;
    }

    public override async Task<ComponentLibraryModel> Delete(int id)
    {
        var entity = await _repository.Get(id);
        var model = await base.Delete(id);
        var location = entity.GetLocation(_settings);

        try
        {
            if (Directory.Exists(location))
                Directory.Delete(location, recursive: true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting library files");

            throw new ModelValidationException("General", "Could not delete library files");
        }

        return model;
    }

    public async Task<ComponentLibraryModel> Sync(string source)
    {
        var entity = await _repository.GetBySource(source);
        var model = _mapper.Map<ComponentLibraryModel>(entity ?? new() { Source = source });

        await EnsureValid(model);

        entity = await _librarySync.Sync(source);
        model = _mapper.Map<ComponentLibraryModel>(entity);

        return model;
    }

    public async Task<ComponentLibraryModel> Compose(string source)
    {
        var entity = await _libraryComposer.Compose(source);

        if (!entity.Success)
            throw new LibraryComposeException(entity.Validations);

        var model = _mapper.Map<ComponentLibraryModel>(entity.ComponentLibrary);

        return model;
    }

    #region NotImplemented

    public override Task<ComponentLibraryModel> Add(ComponentLibraryModel model)
    {
        throw new NotImplementedException("Use the sync method to add libraries");
    }

    public override Task<ComponentLibraryModel> Update(ComponentLibraryModel model)
    {
        throw new NotImplementedException("Use the sync method to update libraries");
    }

    #endregion
}
