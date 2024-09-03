using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Observr;
using ReStack.Common.Exceptions;
using ReStack.Common.Models;
using ReStack.Domain.Settings;
using ReStack.Web.Extensions;

namespace ReStack.Web.Components.Base;

public class BaseComponent : ComponentBase
{
    [CascadingParameter] public IModalService Modal { get; set; } = default!;

    [Parameter] public bool IsLoading { get; set; }

    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public IBroker Broker { get; set; }
    [Inject] public IOptions<WebSettings> WebSettings { get; set; }

    public bool LoadError { get; set; }
    public bool BreadcrumbLoaded { get; set; }

    protected async Task SetLoading(bool isLoading, EventCallback<bool> another)
    {
        await SetLoading(isLoading);
        await another.InvokeAsync(isLoading);
    }

    public async Task SetLoading(bool isLoading)
    {
        IsLoading = isLoading;
        await InvokeAsync(StateHasChanged);
    }

    protected async Task ShowError(Exception exception)
    {
        if (exception is ApiException apiException)
        {
            if (apiException.Type == ErrorModelType.StackFileNotFound)
            {
                await ShowError("Each stack should have a file where the content is stored. This file is automaticaly generated whenever a stack is created. However for this stack, the file is missing.", "Did you remount volumes? Or do any data manipulation? The file only gets deleted when a stack is deleted.", null, []);
            }
        }
        else if (exception is HttpRequestException)
        {
            await ShowError("A connection could not be made to the api.",
                $"Is the api still running? Are the appsettings misconfigured? We're trying to reach the api on following url '{WebSettings.Value.ApiUrl}'. Is it possible to <a href=\"{WebSettings.Value.ApiUrl}\\health\" target=\"_blank\" class=\"link\">reach</a> the given url? If so, can the host of the website reach this url?",
                exception, null);
        }
        else
        {
            await ShowUnhandledError(exception);
        }
    }

    protected async Task StateHasChangedAsync()
    {
        await InvokeAsync(StateHasChanged);
    }

    private async Task ShowError(string issue, string cause, Exception exception, Dictionary<string, Task> actions)
    {
        await Modal.Error((MarkupString)issue, (MarkupString)cause, exception, actions, unhandled: false);
    }

    private async Task ShowUnhandledError(Exception exception)
    {
        await Modal.Error((MarkupString)string.Empty, (MarkupString)string.Empty, exception, null, unhandled: true);
    }
}
