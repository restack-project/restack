using AutoMapper;
using FluentValidation;
using ReStack.Common.Exceptions;
using ReStack.Common.Interfaces.Repositories;

namespace ReStack.Application.Aggregates.Base;

public class BaseAggregate<TModel, TEntity, TRepository>(
    TRepository _repository
        , IValidator<TModel> _validator
        , IMapper _mapper
    ) where TRepository : IBaseRepository<TEntity>
{
    public virtual async Task<TModel> Get(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.Get(id, cancellationToken);
        var model = _mapper.Map<TModel>(entity);

        return model;
    }

    public virtual async Task<List<TModel>> GetAll(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAll(cancellationToken);
        var models = _mapper.Map<List<TModel>>(entities);

        return models;
    }

    public virtual async Task<TModel> Add(TModel model)
    {
        await EnsureValid(model);

        var entity = _mapper.Map<TEntity>(model);
        entity = await _repository.Add(entity);
        model = _mapper.Map<TModel>(entity);

        return model;
    }

    public virtual async Task<TModel> Update(TModel model)
    {
        await EnsureValid(model);

        var entity = _mapper.Map<TEntity>(model);
        entity = await _repository.Update(entity);
        model = _mapper.Map<TModel>(entity);

        return model;
    }

    public virtual async Task<TModel> Delete(int id)
    {
        var entity = await _repository.Delete(id);
        var model = _mapper.Map<TModel>(entity);
        return model;
    }


    protected async Task EnsureValid(TModel model)
    {
        var validationResult = await _validator.ValidateAsync(model);

        if (!validationResult.IsValid)
        {
            throw new ModelValidationException(validationResult);
        }
    }
}
