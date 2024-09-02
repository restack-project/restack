using Microsoft.AspNetCore.Components;

namespace ReStack.Web.Components;

public partial class PanelSelector
{
    private List<Panel> _panels = [];

    [Parameter] public RenderFragment ChildContent { get; set; }

    public Panel ActivePanel { get; set; }

    public async Task AddPage(Panel panel)
    {
        _panels.Add(panel);
        _panels = [.. _panels.OrderBy(x => x.Sequence)];

        if (_panels.Count == 1)
        {
            ActivePanel = panel;
        }

        await StateHasChangedAsync();
    }

    private async Task SelectPanel(Panel panel)
    {
        ActivePanel = panel;

        await StateHasChangedAsync();
    }
}
