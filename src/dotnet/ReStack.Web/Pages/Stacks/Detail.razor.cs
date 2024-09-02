﻿using Microsoft.AspNetCore.Components;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;
using ReStack.Web.Extensions;

namespace ReStack.Web.Pages.Stacks;

public partial class Detail
{
    private int _skipJobs = 0;
    private readonly int _takeJobs = 20;
    private readonly object _jobLock = new();

    [Inject] public IStackClient StackClient { get; set; }
    [Inject] public IJobClient JobClient { get; set; }

    [Parameter] public string QueryStackId { get; set; }

    public int StackId { get; set; }
    public StackModel Stack { get; set; }
    public List<JobModel> Jobs { get; set; } = [];
    public bool IsLoadingJobs { get; set; }

    public override async Task OnStackChanged(StackModel model)
    {
        if (StackId == model.Id)
        {
            Stack.SuccesPercentage = model.SuccesPercentage;
            Stack.AverageRuntime = model.AverageRuntime;

            await StateHasChangedAsync();
        }
    }

    public override async Task OnJobChanged(JobModel model, bool deleted)
    {
        if (StackId == model.StackId)
        {
            lock (_jobLock)
            {
                var existingJob = Jobs.FirstOrDefault(x => x.Id == model.Id);

                if (existingJob is not null)
                {
                    var index = Jobs.IndexOf(existingJob);

                    Jobs.RemoveAt(index);

                    if (!deleted)
                    {
                        Jobs.Insert(index, model);
                    }
                }
                else if (!deleted)
                {
                    Jobs.Add(model);
                    Jobs = [.. Jobs.OrderByDescending(x => x.Started)];
                }
            }

            await StateHasChangedAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await SetLoading(true);

            BreadcrumbLinks = new() { { "Stacks", NavigationManager.Stacks() } };

            if (int.TryParse(QueryStackId, out var stackId))
            {
                StackId = stackId;

                Stack = await StackClient.Get(StackId);

#pragma warning disable CS4014
                Task.Run(LoadMoreJobs);
#pragma warning restore CS4014

                BreadcrumbLinks.Add(Stack.Name, NavigationManager.StackDetail(StackId));
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
        finally
        {
            await SetLoading(false);
        }

        await base.OnInitializedAsync();
    }

    private async Task LoadMoreJobs()
    {
        try
        {
            IsLoadingJobs = true;
            await StateHasChangedAsync();

            var jobs = await JobClient.Take(StackId, _skipJobs, _takeJobs);
            Jobs.AddRange(jobs);

            _skipJobs += _takeJobs;
        }
        catch (Exception ex)
        {
            await ShowError(ex);
        }
        finally
        {
            IsLoadingJobs = false;
            await StateHasChangedAsync();
        }
    }

    private async Task Execute()
    {
        try
        {
            await SetLoading(true);

            var job = await StackClient.Execute(Stack.Id);

            NavigationManager.NavigateToJobDetail(StackId, job.Id);
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
