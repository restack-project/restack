namespace ReStack.Common.Interfaces.Repositories;

public interface IBaseRepository<TE>
{
    Task<TE> Get(int id, CancellationToken cancellationToken = default);
    Task<List<TE>> GetAll(CancellationToken cancellationToken = default);
    Task<TE> Add(TE entity);
    Task<TE> Update(TE entity);
    Task<TE> Delete(int id);
}
