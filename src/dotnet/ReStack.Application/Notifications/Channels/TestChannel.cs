using ReStack.Common.Interfaces;
using ReStack.Common.Models;

namespace ReStack.Application.Notifications.Channels;

public class TestChannel : INotificationChannel
{
    public async Task<StackModel> StackChanged(StackModel stack)
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        return stack;
    }

    public async Task<JobModel> JobChanged(JobModel job)
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        return job;
    }

    public async Task<LogModel> LogAdded(LogModel log)
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        return log;
    }

    public async Task<JobModel> JobDeleted(JobModel job)
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        return job;
    }
}
