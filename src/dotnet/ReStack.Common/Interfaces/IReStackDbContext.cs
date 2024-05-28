using Microsoft.EntityFrameworkCore;
using ReStack.Domain.Entities;

namespace ReStack.Common.Interfaces;

public interface IReStackDbContext
{
    DbSet<Stack> Stack { get; set; }
    DbSet<Job> Job { get; set; }
    DbSet<Log> Log { get; set; }
    DbSet<ComponentLibrary> ComponentLibrary { get; set; }
    DbSet<ComponentParameter> ComponentParameter { get; set; }
    DbSet<Component> Component { get; set; }
    DbSet<StackComponent> StackComponent { get; set; }
    DbSet<StackIgnoreRule> StackIgnoreRule { get; set; }

    void SeedData();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
