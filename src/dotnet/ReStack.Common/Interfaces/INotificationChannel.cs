using ReStack.Common.Models;

namespace ReStack.Common.Interfaces;

public interface INotificationChannel
{
    Task<StackModel> StackChanged(StackModel stack);
    Task<JobModel> JobChanged(JobModel job);
    Task<JobModel> JobDeleted(JobModel job);
    Task<LogModel> LogAdded(LogModel log);
}
