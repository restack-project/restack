using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using ReStack.Domain.Settings;

namespace ReStack.Web.Modals;

public partial class Error
{
    [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IOptions<WebSettings> WebSettings { get; set; }

    [Parameter] public MarkupString Issue { get; set; }
    [Parameter] public MarkupString Cause { get; set; }
    [Parameter] public Exception Exception { get; set; }
    [Parameter] public Dictionary<string, Task> Actions { get; set; } = [];
    [Parameter] public bool Unhandled { get; set; }

    public bool ShowTechincalError { get; private set; }

    private async Task ToggleTechnicalError()
    {
        ShowTechincalError = !ShowTechincalError;
        await InvokeAsync(StateHasChanged);
    }
}
