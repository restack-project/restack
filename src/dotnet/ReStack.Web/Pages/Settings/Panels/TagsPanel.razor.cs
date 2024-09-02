using Microsoft.AspNetCore.Components;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;
using ReStack.Web.Extensions;

namespace ReStack.Web.Pages.Settings.Panels;

public partial class TagsPanel
{
    [Inject] public ITagClient TagClient { get; set; }

    public List<TagModel> Tags { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadTags();
        await base.OnInitializedAsync();
    }

    private async Task LoadTags()
    {
        try
        {
            await SetLoading(true);

            Tags = await TagClient.GetAll();
        }
        catch (Exception e)
        {
            await ShowError(e);
        }
        finally
        {
            await SetLoading(false);
        }
    }

    private async Task Add()
    {
        var modal = Modal.AddTag();
        var result = await modal.Result;

        if (!result.Cancelled)
        {
            await LoadTags();
        }
    }

    private async Task Delete(TagModel tag)
    {

    }
}
