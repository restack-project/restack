using Microsoft.AspNetCore.Components;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;
using ReStack.Web.Extensions;
using ReStack.Web.Modals;

namespace ReStack.Web.Pages.Stacks;

public partial class Overview
{
    private List<StackModel> _stacks = [];
    private List<TagModel> _tags = [];

    [Inject] public IStackClient StackClient { get; set; }
    [Inject] public IJobClient JobClient { get; set; }
    [Inject] public ITagClient TagClient { get; set; }

    [SupplyParameterFromQuery(Name = "tab")][Parameter] public string QuerySelectedTag { get; set; }
    [SupplyParameterFromQuery(Name = "search")][Parameter] public string SearchText { get; set; }
    
    public IEnumerable<StackModel> Stacks { get; private set; } = [];
    public ICollection<StackModel> SelectedStacks { get; private set; } = [];
    public TagModel SelectedTag { get; private set; }

    public override async Task OnStackChanged(StackModel model)
    {
        var existingStack = _stacks.FirstOrDefault(x => x.Id == model.Id);
        var index = _stacks.IndexOf(existingStack);

        _stacks.RemoveAt(index);
        _stacks.Insert(index, model);

        await StateHasChangedAsync();
    }

    public override async Task OnJobChanged(JobModel model, bool deleted)
    {
        var existingStack = _stacks.FirstOrDefault(x => model.StackId == x.Id);

        if (existingStack is not null)
        {
            var existingJob = existingStack.Jobs.FirstOrDefault(x => x.Id == model.Id);

            if (existingJob is null)
            {
                existingStack.Jobs.Add(model);
            }
            else
            {
                existingStack.Jobs.Remove(existingJob);

                if (!deleted)
                {
                    existingStack.Jobs.Add(model);
                }
            }
        }

        await base.OnJobChanged(model, deleted);
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await SetLoading(true);

            BreadcrumbLinks = new() { { "Stacks", NavigationManager.Stacks() } };

            _stacks = await StackClient.GetAll();
            _tags = await TagClient.GetAll();

            if (!string.IsNullOrWhiteSpace(QuerySelectedTag))
            {
                SelectedTag = _tags.FirstOrDefault(x => x.Id.ToString() == QuerySelectedTag);
            }

            await Search();
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

    private async Task Search()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            Stacks = _stacks;
        }

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            Stacks = _stacks.Where(x => x.Name.Contains(SearchText, StringComparison.CurrentCultureIgnoreCase));
        }

        if (SelectedTag is not null)
        {
            Stacks = Stacks.Where(x => x.Tags.Contains(SelectedTag));
        }

        Stacks = [.. Stacks.OrderBy(x => x.Name)];

        await StateHasChangedAsync();

        await JS.UpdateUrl(NavigationManager.Stacks(SelectedTag?.Id, SearchText));
    }

    private async Task ClearSearch()
    {
        SearchText = string.Empty;

        await Search();
    }

    private async Task SelectTag(TagModel tag)
    {
        SelectedTag = tag;

        await Search();
    }

    private async Task ToggleSelected(StackModel model)
    {
        var existingStack = SelectedStacks.FirstOrDefault(x => x.Id == model.Id);

        if (existingStack is null)
        {
            SelectedStacks.Add(model);
        }
        else
        {
            SelectedStacks.Remove(existingStack);
        }

        await StateHasChangedAsync();
    }

    private async Task ToggleAllSelected(bool forceUnselect = false)
    {
        if (SelectedStacks.Count == Stacks.Count() || forceUnselect)
        {
            SelectedStacks.Clear();
        }
        else
        {
            SelectedStacks = Stacks.ToList();
        }

        await StateHasChangedAsync();
    }

    private async Task Execute()
    {
        if (QuestionResult.Yes == await Modal.Question($"Continue?", $"This will execute {SelectedStacks.Count} stack(s) at the same time."))
        {
            await SetLoading(true);

            foreach (var stack in SelectedStacks)
            {
                try
                {
                    await StackClient.Execute(stack.Id);
                }
                catch (Exception ex)
                {
                    await ShowError(ex);
                }
            }

            await ToggleAllSelected(forceUnselect: true);
        }

        await SetLoading(false);
    }

    private async Task Cancel()
    {
        if (QuestionResult.Yes == await Modal.Question($"Continue?", $"This will cancel all {SelectedStacks.Count} running job(s) at the same time."))
        {
            await SetLoading(true);

            foreach (var stack in SelectedStacks)
            {
                try
                {
                    await StackClient.Cancel(stack.Id);
                }
                catch (Exception ex)
                {
                    await ShowError(ex);
                }
            }

            await ToggleAllSelected(forceUnselect: true);
        }

        await SetLoading(false);
    }
}
