using Microsoft.AspNetCore.SignalR;
using ReStack.Application.Notifications.Hubs;
using ReStack.Common.Interfaces;
using ReStack.Common.Models;

namespace ReStack.Application.Notifications.Channels;

public class SignalRChannel : INotificationChannel
{
    private readonly IHubContext<StackHub> _hubContext;

    public SignalRChannel(IHubContext<StackHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task<StackModel> StackChanged(StackModel stack)
    {
        await _hubContext.Clients.All.SendAsync("StackChanged", stack);
        return stack;
    }

    public async Task<JobModel> JobChanged(JobModel job)
    {
        await _hubContext.Clients.All.SendAsync("JobChanged", job);
        return job;
    }

    public async Task<JobModel> JobDeleted(JobModel job)
    {
        await _hubContext.Clients.All.SendAsync("JobDeleted", job);
        return job;
    }

    public async Task<LogModel> LogAdded(LogModel log)
    {
        await _hubContext.Clients.All.SendAsync("LogAdded", log);
        return log;
    }
}
