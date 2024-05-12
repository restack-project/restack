using Microsoft.EntityFrameworkCore;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Domain.Entities;

namespace ReStack.Persistence.Repositories;

public class LogRepository(
    IReStackDbContext _context
) : ILogRepository
{
    public async Task<Log> Add(Log entity)
    {
        _context.Log.Add(entity);

        await _context.SaveChangesAsync();

        return await Get(entity.Id);
    }

    public async Task<Log> Get(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Log.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity;
    }

    #region NotImplemented

    public Task<Log> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Log>> GetAll(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Log> Update(Log entity)
    {
        throw new NotImplementedException();
    }

    #endregion
}
