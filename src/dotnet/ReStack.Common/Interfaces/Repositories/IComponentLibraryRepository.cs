using ReStack.Domain.Entities;

namespace ReStack.Common.Interfaces.Repositories;

public interface IComponentLibraryRepository : IBaseRepository<ComponentLibrary>
{
    Task<ComponentLibrary> GetBySource(string source, CancellationToken token = default);
    Task<List<Stack>> GetUsingStacks(int id, CancellationToken token = default);
}
