using ReStack.Web.Extensions;

namespace ReStack.Web.Pages.Settings;

public partial class Overview
{
    protected override Task OnInitializedAsync()
    {
        BreadcrumbLinks = new()
        {
            { "Settings", NavigationManager.Settings() }
        };

        return base.OnInitializedAsync();
    }
}
