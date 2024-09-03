using Microsoft.AspNetCore.Components;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;
using ReStack.Web.Extensions;
using ReStack.Web.Modals;
using ReStack.Web.Shared;

namespace ReStack.Web.Pages.Libraries;

public partial class Detail
{
    [Inject] public IComponentLibraryClient ComponentLibraryClient { get; set; }

    [Parameter] public string QueryLibraryId { get; set; }

    public ComponentLibraryModel Library { get; set; }
    public Page Page { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await SetLoading(true);

            if (int.TryParse(QueryLibraryId, out var libraryId))
            {
                Library = await ComponentLibraryClient.Get(libraryId);
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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!BreadcrumbLoaded && !IsLoading && !LoadError)
        {
            await Page.Breadcrumb.Add("Libraries", NavigationManager.Libraries());
            await Page.Breadcrumb.Add(Library.Name, NavigationManager.LibraryDetail(Library.Id));

            BreadcrumbLoaded = true;
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task Sync()
    {
        try
        {
            await SetLoading(true);

            Library = await ComponentLibraryClient.Sync(Library.Source);

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
        try
        {
            var stacksInUse = await ComponentLibraryClient.GetUsingStacks(Library.Id);
            var body = stacksInUse is not null && stacksInUse.Any() ?
                $"Following stacks will be affected by deleting this library: {string.Join(',', stacksInUse.Select(x => $"'{x.Name}'"))}" : string.Empty;
            var answer = await Modal.Question($"Delete library '{Library.Name}'?", body);

            if (answer == QuestionResult.Yes)
            {
                await SetLoading(true);

                await ComponentLibraryClient.Delete(Library.Id);

                NavigationManager.NavigateToLibraries();
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
}
