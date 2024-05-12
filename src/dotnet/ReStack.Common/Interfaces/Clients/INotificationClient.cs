namespace ReStack.Common.Interfaces.Clients;

public interface INotificationClient
{
    NotificationState State { get; }

    Task Connect(Func<Task> stateChanged, CancellationToken token = default);
}

public enum NotificationState
{
    Connected, Connecting, Disconnected, Reconnecting
}
