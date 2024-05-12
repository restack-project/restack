using Microsoft.EntityFrameworkCore;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Domain.Entities;
using ReStack.Domain.ValueObjects;

namespace ReStack.Persistence.Repositories;

public class StackRepository(
    IReStackDbContext _context
) : IStackRepository
{
    public async Task<Stack> Add(Stack entity)
    {
        entity.Id = 0;
        entity.Jobs = [];

        _context.Stack.Add(entity);

        await _context.SaveChangesAsync();

        return await Get(entity.Id);
    }

    public async Task<Stack> Delete(int id)
    {
        var returnEntity = await Get(id);
        var entity = await _context.Stack.FirstOrDefaultAsync(x => x.Id == id);

        _context.Stack.Remove(entity);

        await _context.SaveChangesAsync();

        return returnEntity;
    }

    public Task<Stack> Get(int id, CancellationToken cancellationToken = default)
    {
        return Get(id, numberOfJobs: 10, cancellationToken: cancellationToken);
    }

    public async Task<Stack> Get(int id, int numberOfJobs, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Stack
            .Include(x => x.Components)
            .ThenInclude(x => x.Component)
            .ThenInclude(x => x.ComponentLibrary)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        var jobs = await _context.Job
            .Include(x => x.Logs)
            .Where(x => x.StackId == id)
            .OrderByDescending(x => x.Started)
            .Take(numberOfJobs)
            .ToListAsync(cancellationToken);

        if (entity is not null)
            entity.Jobs = jobs.ToList();

        return entity;
    }

    public Task<List<Stack>> GetAll(CancellationToken cancellationToken = default)
    {
        return GetAll(numberOfJobs: 1, cancellationToken);
    }

    public async Task<List<Stack>> GetAll(int numberOfJobs, CancellationToken token = default)
    {
        var entities = await _context.Stack
            .Include(x => x.Components)
            .ThenInclude(x => x.Component)
            .ToListAsync(token);

        foreach (var entity in entities)
        {
            entity.Jobs = await _context.Job
                .OrderByDescending(x => x.Started)
                .Where(x => x.StackId == entity.Id)
                .Take(numberOfJobs)
                .Include(x => x.Logs)
                .ToListAsync();
        }

        return entities;
    }

    public async Task<Stack> Update(Stack entity)
    {
        var updatedEntity = await _context.Stack
            .Include(x => x.Components)
            .FirstOrDefaultAsync(x => x.Id == entity.Id);

        updatedEntity.Name = entity.Name;
        updatedEntity.Type = entity.Type;
        updatedEntity.Components = entity.Components;
        updatedEntity.FailOnStdError = entity.FailOnStdError;

        await _context.SaveChangesAsync();

        return await Get(updatedEntity.Id);
    }

    public async Task<Stack> CalculateStats(int stackId, CancellationToken cancellationToken = default)
    {
        var jobs = await _context.Job.Where(x => x.StackId == stackId).ToListAsync(cancellationToken);
        var totalJobs = jobs.Count;
        var totalSuccessJobs = jobs.Count(x => x.State == JobState.Success);
        var totalRuntime = jobs.Where(x => x.Ended.HasValue).Sum(x => (x.Ended.Value - x.Started).TotalSeconds);
        var stack = await _context.Stack.FirstOrDefaultAsync(x => x.Id == stackId, cancellationToken);

        stack.SuccesPercentage = totalJobs != 0 ? Math.Round((decimal)totalSuccessJobs / totalJobs * 100, 2) : 0;
        stack.AverageRuntime = totalJobs != 0 ? Math.Round((decimal)totalRuntime / totalJobs, 2) : 0;

        await _context.SaveChangesAsync(cancellationToken);

        return await Get(stackId, cancellationToken);
    }
}
