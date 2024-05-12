using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ReStack.Web.Extensions;

namespace ReStack.Web.Components;

public partial class ButtonCopy
{
    private bool _showCopied = false;

    [Inject] public IJSRuntime JS { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public string CssClass { get; set; } = "btn-outline-secondary";
    [Parameter] public Func<Task<string>> Value { get; set; }


    private async Task Copy()
    {
        var value = await Value.Invoke();

        await JS.CopyToClipboard(value);

        _showCopied = true;
        await InvokeAsync(StateHasChanged);

        await Task.Delay(TimeSpan.FromSeconds(1));

        _showCopied = false;
        await InvokeAsync(StateHasChanged);
    }
}
