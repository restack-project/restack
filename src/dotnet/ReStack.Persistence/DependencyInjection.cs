using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Persistence.Repositories;

namespace ReStack.Persistence;

// dotnet ef --startup-project ReStack.Api/ migrations add 20240528_1 --project ReStack.Persistence.PostgresMigrations
// dotnet ef --startup-project ../ReStack.Api/ database update

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IReStackDbContext, ReStackDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("ReStack"), x => x.MigrationsAssembly("ReStack.Persistence.PostgresMigrations"))
        );

        services.AddRepositories();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IStackRepository, StackRepository>();
        services.AddTransient<IJobRepository, JobRepository>();
        services.AddTransient<IComponentLibraryRepository, ComponentLibraryRepository>();
        services.AddTransient<ILogRepository, LogRepository>();
        services.AddTransient<ITagRepository , TagRepository>();

        return services;
    }

    public static IHost MigrateDatabase(this IHost app, bool seedData)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ReStackDbContext>();
        dbContext.Database.Migrate();

        if (seedData)
            dbContext.SeedData();

        return app;
    }
}