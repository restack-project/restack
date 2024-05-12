using AutoMapper;
using FluentValidation;
using ReStack.Application.Aggregates.Base;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Aggregates;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Common.Models;
using ReStack.Domain.Entities;

namespace ReStack.Application.Aggregates;

public class JobAggregate(
    IJobRepository _repository
    , IValidator<JobModel> _validator
    , IMapper _mapper
    , IStackRepository _stackRepository
    , ITokenCache _tokenCache
    , INotificationPublisher _notificationPublisher
) : BaseAggregate<JobModel, Job, IJobRepository>(_repository, _validator, _mapper), IJobAggregate
{
    public override async Task<JobModel> Delete(int id)
    {
        var job = await _repository.Get(id);
        var model = await base.Delete(id);
        var stack = await _stackRepository.CalculateStats(model.StackId);

        await _notificationPublisher.JobDeleted(job);
        await _notificationPublisher.StackChanged(stack);

        return model;
    }

    public async Task<JobModel> GetBySequence(int stackId, int sequence, CancellationToken token = default)
    {
        var entity = await _repository.GetBySequence(stackId, sequence, token);
        var model = _mapper.Map<JobModel>(entity);

        return model;
    }

    public async Task<List<JobModel>> Take(int stackId, int skip, int take, CancellationToken token = default)
    {
        var entities = await _repository.Take(stackId, skip, take, token);
        var models = _mapper.Map<List<JobModel>>(entities);

        return models;
    }

    public async Task<JobModel> Cancel(int jobId)
    {
        var token = _tokenCache.Get(jobId);
        var job = await _repository.Get(jobId);

        token?.Cancel();

        if (job.State != Domain.ValueObjects.JobState.Cancelled)
        {
            job.State = Domain.ValueObjects.JobState.Cancelled;

            await _repository.Update(job);
            await _notificationPublisher.JobChanged(job);
        }

        return _mapper.Map<JobModel>(job);
    }
}
