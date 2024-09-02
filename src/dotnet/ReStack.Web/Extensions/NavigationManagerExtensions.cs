using Microsoft.AspNetCore.Components;

namespace ReStack.Web.Extensions;

public static class NavigationManagerExtensions
{
    public static string Stacks(this NavigationManager _, int? selectedTag = null, string search = null) => $"/stacks?tab={selectedTag}&search={search}";
    public static string StackAdd(this NavigationManager _) => "/stacks/add";
    public static string StackEdit(this NavigationManager _, int stackId) => $"/stacks/{stackId}/edit";
    public static string StackDetail(this NavigationManager _, int stackId) => $"/stacks/{stackId}";
    public static string StackJobDetail(this NavigationManager _, int stackId, int jobId) => $"/stacks/{stackId}/job/{jobId}";
    public static string Libraries(this NavigationManager _) => "/libraries";
    public static string LibraryAdd(this NavigationManager _) => "/libraries/add";
    public static string LibraryDetail(this NavigationManager _, int libraryId) => $"/libraries/{libraryId}";
    public static string LibraryEdit(this NavigationManager _, int libraryId) => $"/libraries/{libraryId}/edit";
    public static string Settings(this NavigationManager _) => "/settings";

    public static void NavigateToHome(this NavigationManager navigationManager, bool forceLoad = false)
        => navigationManager.NavigateTo(navigationManager.Stacks(), forceLoad);

    public static void NavigateToStackDetail(this NavigationManager navigationManager, int stackId, bool forceLoad = false)
        => navigationManager.NavigateTo(navigationManager.StackDetail(stackId), forceLoad);

    public static void NavigateToStackEdit(this NavigationManager navigationManager, int stackId, bool forceLoad = false)
    => navigationManager.NavigateTo(navigationManager.StackEdit(stackId), forceLoad);

    public static void NavigateToStackAdd(this NavigationManager navigationManager, bool forceLoad = false)
        => navigationManager.NavigateTo(navigationManager.StackAdd(), forceLoad);

    public static void NavigateToJobDetail(this NavigationManager navigationManager, int stackId, int jobId, bool forceLoad = false)
        => navigationManager.NavigateTo(navigationManager.StackJobDetail(stackId, jobId), forceLoad);

    public static void NavigateToLibraries(this NavigationManager navigationManager, bool forceLoad = false)
        => navigationManager.NavigateTo(navigationManager.Libraries(), forceLoad);

    public static void NavigateToLibrariesAdd(this NavigationManager navigationManager, bool forceLoad = false)
        => navigationManager.NavigateTo(navigationManager.LibraryAdd(), forceLoad);

    public static void NavigateToLibrariesEdit(this NavigationManager navigationManager, int libraryId, bool forceLoad = false)
        => navigationManager.NavigateTo(navigationManager.LibraryEdit(libraryId), forceLoad);

    public static void NavigateToSettings(this NavigationManager navigationManager, bool forceLoad = false)
        => navigationManager.NavigateTo(navigationManager.Settings(), forceLoad);
}
