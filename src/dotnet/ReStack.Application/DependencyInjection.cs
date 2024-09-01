using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Observr;
using ReStack.Application.Aggregates;
using ReStack.Application.Libraries;
using ReStack.Application.Notifications;
using ReStack.Application.StackHandling;
using ReStack.Application.StackHandling.Languages;
using ReStack.Application.StackHandling.Segements;
using ReStack.Application.StackHandling.Validators;
using ReStack.Common.HttpClients;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Aggregates;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;
using ReStack.Common.Validators;
using ReStack.Domain.Settings;
using System.Reflection;

namespace ReStack.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var apiSettings = new ApiSettings();
        configuration.GetSection(nameof(ApiSettings)).Bind(apiSettings);

        services.Configure<ApiSettings>(configuration.GetSection(nameof(ApiSettings)))
                .AddAutoMapper(Assembly.GetExecutingAssembly(), typeof(StackModel).Assembly)
                .AddSingleton<INotificationPublisher, NotificationPublisher>()
                .AddTransient<IStackExecutor, StackPipeline>()
                .AddTransient<ILibraryComposer, LibraryComposer>()
                .AddTransient<ILibrarySync, LibrarySync>()
                .AddHostedService<JobQueueHostedService>()
                .AddSingleton<ITokenCache, TokenCache>()
                .AddSingleton<IJobQueue>(_ =>
                {
                    return new JobQueue(apiSettings?.JobQueue == 0 ? 100 : apiSettings.JobQueue);
                });

        services.AddAggregates()
                .AddBaseClasses<BaseLanguage>()
                .AddBaseClasses<BaseStrategyValidator>(registerAsBase: false)
                .AddBaseClasses<BaseSegment>(registerAsBase: false)
                .AddBaseClasses<INotificationChannel>(registerAsBase: false);

        services.AddValidatorsFromAssemblyContaining<StackModelValidator>();

        return services;
    }

    public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddObservr();
        services.Configure<WebSettings>(configuration.GetSection(nameof(WebSettings)));
        services.AddScoped<INotificationClient, NotificationClient>();
        services.AddHttpClient<IStackClient, StackClient>(RegisterHttpClient(configuration));
        services.AddHttpClient<IJobClient, JobClient>(RegisterHttpClient(configuration));
        services.AddHttpClient<IComponentLibraryClient, ComponentLibraryClient>(RegisterHttpClient(configuration));
        services.AddHttpClient<ISshKeyClient, SshKeyClient>(RegisterHttpClient(configuration));
        services.AddHttpClient<ITagClient, TagClient>(RegisterHttpClient(configuration));

        return services;
    }

    public static IHost GenerateSshKey(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var sshKeyAggregate = scope.ServiceProvider.GetRequiredService<ISshKeyAggregate>();

        var key = sshKeyAggregate.Get().GetAwaiter().GetResult();
        if (key is null)
            sshKeyAggregate.Generate().GetAwaiter().GetResult();

        return app;
    }

    private static IServiceCollection AddAggregates(this IServiceCollection services)
    {
        services.AddTransient<IStackAggregate, StackAggregate>();
        services.AddTransient<IJobAggregate, JobAggregate>();
        services.AddTransient<ISshKeyAggregate, SshKeyAggregate>();
        services.AddTransient<IComponentLibraryAggregate, ComponentLibraryAggregate>();
        services.AddTransient<ITagAggregate, TagAggregate>();

        return services;
    }

    private static Action<IServiceProvider, HttpClient> RegisterHttpClient(IConfiguration configuration)
    {
        return (_, client) =>
        {
            client.BaseAddress = new Uri(configuration.GetSection($"{nameof(WebSettings)}:{nameof(WebSettings.ApiUrl)}").Value);
        };
    }

    private static IServiceCollection AddBaseClasses<TE>(this IServiceCollection services, bool registerAsBase = true, params Type[] ignore)
    {
        var implementationTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(TE).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract && !ignore.Contains(type));

        foreach (var implementationType in implementationTypes)
        {
            if (registerAsBase)
                services.AddTransient(typeof(TE), implementationType);
            else
                services.AddTransient(implementationType);
        }

        return services;
    }
}