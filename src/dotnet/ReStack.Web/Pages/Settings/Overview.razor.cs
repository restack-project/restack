using Microsoft.AspNetCore.Components;
using ReStack.Web.Extensions;
using ReStack.Web.Shared;

namespace ReStack.Web.Pages.Settings;

public partial class Overview
{
    [Parameter] public string QueryPanel { get; set; }

    public Page Page { get; set; }

    protected override async Task OnInitializedAsync()
    {

        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!BreadcrumbLoaded && !IsLoading && !LoadError)
        {
            await Page.Breadcrumb.Add("Settings", NavigationManager.Settings());

            BreadcrumbLoaded = true;
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}
