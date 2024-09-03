using Microsoft.AspNetCore.Components;
using ReStack.Web.Extensions;

namespace ReStack.Web.Pages.Settings;

public partial class Overview
{
    [Parameter] public string QueryPanel { get; set; }

    protected override Task OnInitializedAsync()
    {
        BreadcrumbLinks = new()
        {
            { "Settings", NavigationManager.Settings() }
        };

        return base.OnInitializedAsync();
    }
}
