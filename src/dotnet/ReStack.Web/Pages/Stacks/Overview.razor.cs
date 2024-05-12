using Microsoft.AspNetCore.Components;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;

namespace ReStack.Web.Pages.Stacks;

public partial class Overview
{
    private List<StackModel> _stacks = [];

    [Inject] public IStackClient StackClient { get; set; }
    [Inject] public IJobClient JobClient { get; set; }

    public string SearchText { get; set; }
    public IEnumerable<StackModel> Stacks { get; private set; } = [];

    public override async Task OnStackChanged(StackModel model)
    {
        var existingStack = _stacks.FirstOrDefault(x => x.Id == model.Id);
        var index = _stacks.IndexOf(existingStack);

        _stacks.RemoveAt(index);
        _stacks.Insert(index, model);

        await StateHasChangedAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await SetLoading(true);

            BreadcrumbLinks = new() { { "Stacks", "/stacks" } };

            _stacks = await StackClient.GetAll();

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

        Stacks = [.. Stacks.OrderBy(x => x.Name)];

        await StateHasChangedAsync();
    }

    private async Task ClearSearch()
    {
        SearchText = string.Empty;

        await Search();
    }
}
