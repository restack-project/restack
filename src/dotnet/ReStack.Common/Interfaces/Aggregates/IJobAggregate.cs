using ReStack.Common.Models;

namespace ReStack.Common.Interfaces.Aggregates;

public interface IJobAggregate : IBaseAggregate<JobModel>
{
    Task<List<JobModel>> Take(int stackId, int skip, int take, CancellationToken token = default);
    Task<JobModel> GetBySequence(int stackId, int sequence, CancellationToken token = default);
    Task<JobModel> Cancel(int jobId);
}
