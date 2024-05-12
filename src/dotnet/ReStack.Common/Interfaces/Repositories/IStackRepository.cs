using ReStack.Domain.Entities;

namespace ReStack.Common.Interfaces.Repositories;

public interface IStackRepository : IBaseRepository<Stack>
{
    Task<List<Stack>> GetAll(int numberOfJobs = 1, CancellationToken token = default);
    Task<Stack> Get(int id, int numberOfJobs, CancellationToken cancellationToken = default);
    Task<Stack> CalculateStats(int stackId, CancellationToken cancellationToken = default);
}
