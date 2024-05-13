using Microsoft.AspNetCore.Components;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;
using ReStack.Web.Extensions;
using ReStack.Web.Modals;
using ReStack.Web.Shared;

namespace ReStack.Web.Pages.Stacks;

public partial class Execute
{
    private readonly object _logLock = new();

    [Inject] public IStackClient StackClient { get; set; }
    [Inject] public IJobClient JobClient { get; set; }

    [Parameter] public string QueryStackId { get; set; }
    [Parameter] public string QueryJobId { get; set; }

    public int StackId { get; set; }
    public int? JobId { get; set; }
    public StackModel Stack { get; set; }
    public JobModel Job { get; set; }
    public Page Page { get; set; }

    public override async Task OnLogAdded(LogModel log)
    {
        if (log.JobId == Job?.Id)
        {
            lock (_logLock)
            {
                if (!Job.Logs.Any(x => x.Id == log.Id))
                {
                    Job.Logs.Add(log);
                }

                Job.Logs = [.. Job.Logs.OrderByDescending(x => x.Timestamp)];
            }

            await StateHasChangedAsync();
        }
    }

    public override async Task OnJobChanged(JobModel model, bool deleted)
    {
        if (model.Id == JobId)
        {
            Job = model;

            await StateHasChangedAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            BreadcrumbLinks = new() { { "Stacks", "/stacks" } };

            if (int.TryParse(QueryStackId, out var stackId) && int.TryParse(QueryJobId, out var jobId))
            {
                StackId = stackId;
                JobId = jobId;

                Job = await JobClient.Get(JobId.Value);
                Stack = await StackClient.Get(StackId);

                if (Job is not null)
                {
                    BreadcrumbLinks.Add(Stack.Name, $"stacks/{StackId}");
                    BreadcrumbLinks.Add($"#{Job.Sequence}", $"execute/{JobId}");
                }
                else
                {
                    LoadError = true;
                }
            }
            else
            {
                LoadError = true;
            }
        }
        catch (Exception ex)
        {
            await ShowError(ex);
        }

        await base.OnInitializedAsync();
    }

    private async Task ExecuteNew()
    {
        try
        {
            await SetLoading(true);

            var job = await StackClient.Execute(Stack.Id);

            NavigationManager.NavigateToJobDetail(StackId, job.Id, forceLoad: true);
        }
        catch (Exception ex)
        {
            await ShowError(ex);
        }
        finally
        {
            await SetLoading(false);
        }
    }

    private async Task Cancel()
    {
        try
        {
            await SetLoading(true);

            if (await Modal.Question($"Cancel job #{Job.Sequence}?") == Modals.QuestionResult.Yes)
            {
                Job = await JobClient.Cancel(Job.Id);
            }
        }
        catch (Exception ex)
        {
            await ShowError(ex);
        }
        finally
        {
            await SetLoading(false);
        }
    }

    private async Task Delete()
    {
        var answer = await Modal.Question($"Delete job #{Job.Sequence}?", $"This will delete all logs from the job. This cannot be undone.");

        if (answer == QuestionResult.Yes)
        {
            try
            {
                await SetLoading(true);

                await JobClient.Delete(Job.Id);

                NavigationManager.NavigateToStackDetail(StackId);
            }
            catch (Exception ex)
            {
                await ShowError(ex);
            }
            finally
            {
                await SetLoading(false);
            }
        }
    }
}
