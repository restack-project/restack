using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;
using ReStack.Web.Extensions;
using System.Diagnostics.Eventing.Reader;

namespace ReStack.Web.Pages.Settings.Panels;

public partial class TagsPanel
{
    [Inject] public ITagClient TagClient { get; set; }

    public override string Url { get => "settings/tags"; }
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
        var result = await Modal.AddTag();

        if (result is not null)
        {
            await LoadTags();
        }
    }

    private async Task Edit(TagModel tag)
    {
        var result = await Modal.AddTag(tag);

        if (result is not null)
        {
            await LoadTags();
        }
    }

    private async Task Delete(TagModel tag)
    {
        if (await Modal.Question($"Delete tag '{tag.Name}'?", "If this tag is used on stack(s), it will no longer be visible.") == Modals.QuestionResult.Yes)
        {
            try
            {
                await SetLoading(true);

                await TagClient.Delete(tag.Id);

                await LoadTags();
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
    }
}
