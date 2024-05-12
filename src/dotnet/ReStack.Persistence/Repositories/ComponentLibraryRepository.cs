using Microsoft.EntityFrameworkCore;
using ReStack.Common.Helpers;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Domain.Entities;

namespace ReStack.Persistence.Repositories;

public class ComponentLibraryRepository(
    IReStackDbContext _context
) : IComponentLibraryRepository
{
    public async Task<ComponentLibrary> Add(ComponentLibrary entity)
    {
        _context.ComponentLibrary.Add(entity);

        await _context.SaveChangesAsync();

        return await Get(entity.Id);
    }

    public async Task<ComponentLibrary> Delete(int id)
    {
        var entity = await _context.ComponentLibrary.FirstOrDefaultAsync(x => x.Id == id);
        
        _context.ComponentLibrary.Remove(entity);
        
        await _context.SaveChangesAsync();
        
        return entity;
    }

    public async Task<ComponentLibrary> Get(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.ComponentLibrary
            .Include(x => x.Components)
            .ThenInclude(x => x.Stacks)
            .Include(x => x.Components)
            .ThenInclude(x => x.Parameters)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity;
    }

    public async Task<ComponentLibrary> GetBySource(string source, CancellationToken cancellationToken = default)
    {
        var slug = GitHelper.GetSlug(source);
        var entity = await _context.ComponentLibrary
            .Include(x => x.Components)
            .ThenInclude(x => x.Parameters)
            .FirstOrDefaultAsync(x => x.Slug.ToLower() == slug.ToLower(), cancellationToken);

        return entity;
    }

    public async Task<List<ComponentLibrary>> GetAll(CancellationToken cancellationToken = default)
    {
        var entities = await _context.ComponentLibrary
            .Include(x => x.Components)
            .ThenInclude(x => x.Parameters)
            .ToListAsync(cancellationToken);

        return entities;
    }

    public async Task<List<Stack>> GetUsingStacks(int id, CancellationToken token = default)
    {
        var stacks = await _context.Stack.Where(x => x.Components.Any(x => x.Component.ComponentLibraryId == id)).ToListAsync(token);

        return stacks;
    }

    public async Task<ComponentLibrary> Update(ComponentLibrary entity)
    {
        var updatedEntity = await _context.ComponentLibrary
            .Include(x => x.Components)
            .FirstOrDefaultAsync(x => x.Id == entity.Id);

        updatedEntity.Version = entity.Version;
        updatedEntity.Source = entity.Source;
        updatedEntity.Name = entity.Name;
        updatedEntity.Type = entity.Type;
        updatedEntity.CodeOwners = entity.CodeOwners;
        updatedEntity.Documentation = entity.Documentation;
        updatedEntity.Slug = entity.Slug;
        updatedEntity.LastHashCommit = entity.LastHashCommit;

        MapComponents(entity, updatedEntity);

        await _context.SaveChangesAsync();

        return await Get(updatedEntity.Id);
    }

    private static void MapComponents(ComponentLibrary entity, ComponentLibrary updatedEntity)
    {
        foreach (var component in entity.Components)
        {
            var updatedComponent = updatedEntity.Components.FirstOrDefault(x => x.Id == component.Id);
            if (updatedComponent is null)
            {
                updatedComponent = new() { ComponentLibraryId = updatedEntity.Id };
                updatedEntity.Components.Add(updatedComponent);
            }

            updatedComponent.FileName = component.FileName;
            updatedComponent.FolderName = component.FolderName;
            updatedComponent.Name = component.Name;
            updatedComponent.Parameters = component.Parameters;
        }

        var componentIds = entity.Components.Select(x => x.Id);
        foreach (var updatedComponent in updatedEntity.Components)
        {
            if (!componentIds.Contains(updatedComponent.Id))
                updatedEntity.Components.Remove(updatedComponent);
        }
    }
}
