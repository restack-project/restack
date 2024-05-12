using AutoMapper;
using ReStack.Common.Interfaces;
using ReStack.Common.Models;
using ReStack.Domain.Entities;

namespace ReStack.Application.Notifications;

public class NotificationPublisher : INotificationPublisher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    private readonly List<INotificationChannel> _channels = new();

    public NotificationPublisher(IServiceProvider serviceProvider, IMapper mapper)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        CreateChannels();
    }

    public Task<Stack> StackChanged(Stack stack)
    {
        var model = _mapper.Map<StackModel>(stack);

        FireAndForget((x) => x.StackChanged(model));

        return Task.FromResult(stack);
    }

    public Task<Job> JobChanged(Job job)
    {
        var model = _mapper.Map<JobModel>(job);

        FireAndForget((x) => x.JobChanged(model));

        return Task.FromResult(job);
    }

    public Task<Log> LogAdded(Log log)
    {
        var model = _mapper.Map<LogModel>(log);

        FireAndForget((x) => x.LogAdded(model));

        return Task.FromResult(log);
    }

    public Task<Job> JobDeleted(Job job)
    {
        var model = _mapper.Map<JobModel>(job);

        FireAndForget((x) => x.JobDeleted(model));

        return Task.FromResult(job);
    }

    private void FireAndForget(Action<INotificationChannel> @event)
    {
        foreach (var channel in _channels)
        {
            Task.Run(() =>
            {
                @event(channel);

            }).ConfigureAwait(false);
        }
    }

    private void CreateChannels()
    {
        var implementationTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(INotificationChannel).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract && type != this.GetType());

        foreach (var implementationType in implementationTypes)
        {
            var channel = (INotificationChannel)_serviceProvider.GetService(implementationType);
            _channels.Add(channel);
        }
    }
}
