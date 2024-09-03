using Microsoft.AspNetCore.Components;
using ReStack.Web.Components.Base;

namespace ReStack.Web.Components;

public partial class Panel : BaseComponent
{
    [CascadingParameter] private PanelSelector Parent { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public string CssClass { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public int Sequence { get; set; }
    
    public virtual string Url { get; }
    
    protected bool Visible { get => Parent is null || Parent?.ActivePanel == this; }

    protected override async Task OnInitializedAsync()
    {
        if (Parent is not null)
        {
            await Parent.AddPage(this);
        }

        await base.OnInitializedAsync();
    }
}
