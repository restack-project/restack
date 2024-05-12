using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Observr;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;
using ReStack.Domain.Settings;

namespace ReStack.Common.HttpClients;

public class NotificationClient(
    IOptions<WebSettings> options
    , IBroker broker
) : INotificationClient
{
    private readonly WebSettings _settings = options.Value;
    private HubConnection _connection;

    public NotificationState State { get => MapState(); }

    public async Task Connect(Func<Task> stateChanged, CancellationToken token = default)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(_settings.ApiUrl + "/hub/stack")
            .WithAutomaticReconnect()
            .Build();

        _connection.On<StackModel>("StackChanged", NotifyStackChanged);
        _connection.On<JobModel>("JobChanged", NotifyJobChanged);
        _connection.On<JobModel>("JobDeleted", NotifyJobDeleted);
        _connection.On<LogModel>("LogAdded", NotifyLogAdded);

        _connection.Reconnecting -= (e) => { return stateChanged.Invoke(); };
        _connection.Reconnecting += (e) => { return stateChanged.Invoke(); };
        _connection.Closed -= (e) => { return stateChanged.Invoke(); };
        _connection.Closed += (e) => { return stateChanged.Invoke(); };

        await _connection.StartAsync(token);
    }

    private async Task NotifyStackChanged(StackModel model)
    {
        await broker.Publish(new StackChangedEvent(model));
    }

    private async Task NotifyJobChanged(JobModel model)
    {
        await broker.Publish(new JobChangedEvent(model, deleted: false));
    }

    private async Task NotifyJobDeleted(JobModel model)
    {
        await broker.Publish(new JobChangedEvent(model, deleted: true));
    }

    private async Task NotifyLogAdded(LogModel model)
    {
        await broker.Publish(new LogAddedEvent(model));
    }

    private NotificationState MapState()
    {
        return _connection.State switch
        {
            HubConnectionState.Connected => NotificationState.Connected,
            HubConnectionState.Reconnecting => NotificationState.Reconnecting,
            HubConnectionState.Connecting => NotificationState.Connecting,
            _ => NotificationState.Disconnected,
        };
    }
}

// todo move events to seperate file
public class StackChangedEvent
{
    public StackModel Model { get; }

    public StackChangedEvent(StackModel model)
    {
        Model = model;
    }
}

public class JobChangedEvent
{
    public JobModel Model { get; }
    public bool Deleted { get; set; }

    public JobChangedEvent(JobModel model, bool deleted)
    {
        Model = model;
        Deleted = deleted;
    }
}

public class LogAddedEvent
{
    public LogModel Model { get; }

    public LogAddedEvent(LogModel model)
    {
        Model = model;
    }
}
