using ReStack.Common.HttpClients;
using ReStack.Common.Models;

namespace ReStack.Web.Components.Base;

public class SubscribeComponent
    : BaseComponent, Observr.IObserver<StackChangedEvent>, Observr.IObserver<JobChangedEvent>, Observr.IObserver<LogAddedEvent>, IDisposable
{
    private IDisposable _jobChangedSubscription;
    private IDisposable _stackChangedSubscription;
    private IDisposable _logAddedSubscription;

    public Task Handle(JobChangedEvent value, CancellationToken cancellationToken) => OnJobChanged(value.Model, value.Deleted);

    public Task Handle(LogAddedEvent value, CancellationToken cancellationToken) => OnLogAdded(value.Model);

    public Task Handle(StackChangedEvent value, CancellationToken cancellationToken) => OnStackChanged(value.Model);

    public virtual Task OnStackChanged(StackModel model) { return Task.CompletedTask; }
    public virtual Task OnLogAdded(LogModel model) { return Task.CompletedTask; }
    public virtual Task OnJobChanged(JobModel model, bool deleted) { return Task.CompletedTask; }

    protected override Task OnInitializedAsync()
    {
        _jobChangedSubscription = Broker.Subscribe<JobChangedEvent>(this);
        _stackChangedSubscription = Broker.Subscribe<StackChangedEvent>(this);
        _logAddedSubscription = Broker.Subscribe<LogAddedEvent>(this);

        return base.OnInitializedAsync();
    }

    public void Dispose()
    {
        _stackChangedSubscription?.Dispose();
        _jobChangedSubscription?.Dispose();
        _logAddedSubscription?.Dispose();
    }
}
