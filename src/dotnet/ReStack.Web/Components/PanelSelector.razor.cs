using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ReStack.Web.Extensions;

namespace ReStack.Web.Components;

public partial class PanelSelector
{
    private List<Panel> _panels = [];

    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public EventCallback OnPanelChanged { get; set; }
    [Parameter] public string QueryPanel { get; set; }

    public Panel ActivePanel { get; set; }

    public async Task AddPage(Panel panel)
    {
        _panels.Add(panel);
        _panels = [.. _panels.OrderBy(x => x.Sequence)];

        if (!string.IsNullOrWhiteSpace(panel.Url) && NavigationManager.Uri.Split('?')[0].EndsWith(panel.Url))
        {
            ActivePanel = panel;
        }

        await StateHasChangedAsync();
    }

    private async Task SelectPanel(Panel panel)
    {
        ActivePanel = panel;

        await OnPanelChanged.InvokeAsync();

        if (!string.IsNullOrWhiteSpace(panel.Url))
        {
            var queryParameters = NavigationManager.Uri.Split('?');
            var url = panel.Url;

            if (queryParameters.Length > 1)
            {
                url = $"{url}?{queryParameters[1]}";
            }

            await JS.UpdateUrl(url);

            //BreadcrumbLinks.Add(panel.Title, panel.Url);
        }

        await StateHasChangedAsync();
    }
}
