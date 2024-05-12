using Microsoft.AspNetCore.Components;

namespace ReStack.Web.Components;

public partial class Panel
{
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public string CssClass { get; set; }
}
