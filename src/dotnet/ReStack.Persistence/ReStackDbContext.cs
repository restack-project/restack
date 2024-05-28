using Microsoft.EntityFrameworkCore;
using ReStack.Common.Interfaces;
using ReStack.Domain.Entities;

namespace ReStack.Persistence;

public class ReStackDbContext(
    DbContextOptions<ReStackDbContext> options
) : DbContext(options), IReStackDbContext
{
    public DbSet<Stack> Stack { get; set; }
    public DbSet<Job> Job { get; set; }
    public DbSet<Log> Log { get; set; }
    public DbSet<ComponentLibrary> ComponentLibrary { get; set; }
    public DbSet<ComponentParameter> ComponentParameter { get; set; }
    public DbSet<Component> Component { get; set; }
    public DbSet<StackComponent> StackComponent { get; set; }
    public DbSet<StackIgnoreRule> StackIgnoreRule { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StackComponent>().HasKey(x => new { x.StackId, x.ComponentId });

        modelBuilder.Entity<ComponentLibrary>().HasMany(x => x.Components).WithOne(x => x.ComponentLibrary).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Component>().HasMany(x => x.Parameters).WithOne(x => x.Component).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Component>().HasMany(x => x.Stacks).WithOne(x => x.Component).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Stack>().HasMany(x => x.Components).WithOne(x => x.Stack).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Stack>().HasMany(x => x.IgnoreRules).WithOne(x => x.Stack).OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }

    public void SeedData()
    {
        if (!Stack.Any())
        {
            Stack.Add(new()
            {
                Name = "Windows Bat Stack",
                Type = Domain.ValueObjects.ProgrammingLanguage.Bat
            });
            Stack.Add(new()
            {
                Name = "Linux Shell Stack",
                Type = Domain.ValueObjects.ProgrammingLanguage.Shell
            });
            Stack.Add(new()
            {
                Name = "SSH Stack",
                Type = Domain.ValueObjects.ProgrammingLanguage.Shell
            });
        }

        SaveChanges();
    }
}
