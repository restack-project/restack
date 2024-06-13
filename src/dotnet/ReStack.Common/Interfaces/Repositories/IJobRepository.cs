using ReStack.Domain.Entities;

namespace ReStack.Common.Interfaces.Repositories;

public interface IJobRepository : IBaseRepository<Job>
{
    Task<List<Job>> Take(int jobId, int skip, int take, CancellationToken token = default);
    Task<int> NextSequence(int stackId, CancellationToken token = default);
    Task<Job> GetBySequence(int stackId, int sequence, CancellationToken token = default);
    Task<Job> GetFirstRunningJob(int stackId, CancellationToken token = default);
}
