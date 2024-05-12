namespace ReStack.Web.Pages.Settings;

public partial class Overview
{
    protected override Task OnInitializedAsync()
    {
        BreadcrumbLinks = new()
        {
            { "Settings", "/settings" }
        };

        return base.OnInitializedAsync();
    }
}
