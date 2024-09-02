using Microsoft.EntityFrameworkCore;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Domain.Entities;

namespace ReStack.Persistence.Repositories;

public class TagRepository(
    IReStackDbContext _context
) : ITagRepository
{
    public async Task<Tag> Add(Tag entity)
    {
        entity.Id = 0;

        _context.Tag.Add(entity);

        await _context.SaveChangesAsync();

        return await Get(entity.Id);
    }

    public async Task<Tag> Delete(int id)
    {
        var entity = await _context.Tag.FindAsync(id);

        if (entity is not null)
        {
            _context.Tag.Remove(entity);

            await _context.SaveChangesAsync();
        }

        return entity;
    }

    public async Task<Tag> Get(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Tag.FindAsync(id, cancellationToken);

        return entity;
    }

    public async Task<List<Tag>> GetAll(CancellationToken cancellationToken = default)
    {
        var entities = await _context.Tag.ToListAsync(cancellationToken);

        return entities;
    }

    public async Task<Tag> Update(Tag entity)
    {
        var db = await _context.Tag.FindAsync(entity.Id);

        db.Name = entity.Name;
        db.HexColor = entity.HexColor;

        await _context.SaveChangesAsync();

        return await Get(entity.Id);
    }
}
