using Microsoft.EntityFrameworkCore;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Domain.Entities;
using ReStack.Domain.ValueObjects;

namespace ReStack.Persistence.Repositories;

public class JobRepository(
    IReStackDbContext _context
) : IJobRepository
{
    #region Custom

    public async Task<Job> GetFirstRunningJob(int stackId, CancellationToken token = default)
    {
        var entity = await _context.Job
            .OrderBy(x => x.Sequence)
            .FirstOrDefaultAsync(x => x.StackId == stackId && (x.State == JobState.Queued || x.State == JobState.Running), cancellationToken: token);

        return entity;
    }

    public async Task<Job> GetBySequence(int stackId, int sequence, CancellationToken token = default)
    {
        var entity = await _context.Job
            .Include(x => x.Logs)
            .FirstOrDefaultAsync(x => x.StackId == stackId && x.Sequence == sequence, token);

        return entity;
    }

    public async Task<int> NextSequence(int stackId, CancellationToken token = default)
    {
        var hasJobs = await _context.Job.AnyAsync(x => x.StackId == stackId, token);
        var sequence = !hasJobs ? 0 : await _context.Job.Where(x => x.StackId == stackId).MaxAsync(y => y.Sequence, token);

        return ++sequence;
    }

    public async Task<List<Job>> Take(int stackId, int skip, int take, CancellationToken token = default)
    {
        var jobs = await _context.Job
            .Include(x => x.Logs)
            .OrderByDescending(x => x.Started)
            .Where(x => x.StackId == stackId)
            .Skip(skip).Take(take)
            .ToListAsync(token);

        return jobs;
    }

    #endregion

    #region Default

    public async Task<Job> Add(Job entity)
    {
        entity.Id = 0;

        _context.Job.Add(entity);

        await _context.SaveChangesAsync();

        return await Get(entity.Id);
    }

    public async Task<Job> Delete(int id)
    {
        var returnEntity = await Get(id);

        var entity = await _context.Job.FirstOrDefaultAsync(x => x.Id == id);

        _context.Job.Remove(entity);

        await _context.SaveChangesAsync();

        return returnEntity;
    }

    public async Task<Job> Get(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Job
            .Include(x => x.Logs)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity;
    }

    public async Task<List<Job>> GetAll(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Job.ToListAsync(cancellationToken);

        return entities;
    }

    public async Task<Job> Update(Job entity)
    {
        var updatedEntity = await _context.Job.FirstOrDefaultAsync(x => x.Id == entity.Id);

        updatedEntity.Ended = entity.Ended;
        updatedEntity.Started = entity.Started;
        updatedEntity.StackId = entity.StackId;
        updatedEntity.Logs = entity.Logs;
        updatedEntity.State = entity.State;

        await _context.SaveChangesAsync();

        return await Get(entity.Id);
    }

    #endregion
}
