using Microsoft.AspNetCore.Components;

namespace ReStack.Web.Extensions;

public static class NavigationManagerExtensions
{
    public static void NavigateToHome(this NavigationManager navigationManager, bool forceLoad = false)
    {
        navigationManager.NavigateTo("/", forceLoad);
    }

    public static void NavigateToStackDetail(this NavigationManager navigationManager, int stackId, bool forceLoad = false)
    {
        navigationManager.NavigateTo($"/stacks/{stackId}", forceLoad);
    }

    public static void NavigateToStackAdd(this NavigationManager navigationManager, bool forceLoad = false)
    {
        navigationManager.NavigateTo($"/stacks/add", forceLoad);
    }

    public static void NavigateToJobDetail(this NavigationManager navigationManager, int stackId, int jobId, bool forceLoad = false)
    {
        navigationManager.NavigateTo($"/stacks/{stackId}/job/{jobId}", forceLoad);
    }

    public static void NavigateToLibraries(this NavigationManager navigationManager, bool forceLoad = false)
    {
        navigationManager.NavigateTo("/libraries", forceLoad);
    }
}
