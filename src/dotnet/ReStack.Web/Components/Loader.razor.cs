using Microsoft.AspNetCore.Components;

namespace ReStack.Web.Components;

public partial class Loader
{
    [Parameter] public bool Visible { get; set; }
    [Parameter] public bool ShowOverlay { get; set; } = true;
    [Parameter] public LoaderSize Size { get; set; } = LoaderSize.Medium;

    public enum LoaderSize { Small = 16, Medium = 36, Large = 64 }
}
