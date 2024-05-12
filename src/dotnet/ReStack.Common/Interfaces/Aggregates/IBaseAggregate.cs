namespace ReStack.Common.Interfaces.Aggregates;

public interface IBaseAggregate<TE>
{
    Task<TE> Get(int id, CancellationToken cancellationToken = default);
    Task<List<TE>> GetAll(CancellationToken cancellationToken = default);
    Task<TE> Add(TE model);
    Task<TE> Update(TE model);
    Task<TE> Delete(int id);
}
