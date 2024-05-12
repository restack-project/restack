using ReStack.Domain.Entities;

namespace ReStack.Common.Interfaces;

public interface INotificationPublisher
{
    Task<Stack> StackChanged(Stack stack);
    Task<Job> JobChanged(Job job);
    Task<Job> JobDeleted(Job job);
    Task<Log> LogAdded(Log log);
}
