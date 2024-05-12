using Microsoft.JSInterop;

namespace ReStack.Web.Extensions;

public static class IJsRuntimeExtensions
{
    public static async ValueTask UpdateUrl(this IJSRuntime js, string url)
    {
        try
        {
            var currentUrl = await js.InvokeAsync<string>("window.GetCurrentUrl");
            if (currentUrl != url)
            {
                await js.InvokeVoidAsync("history.pushState", null, "", url.ToLower());
            }
        }
        catch { }
        await ValueTask.CompletedTask;
    }

    public static ValueTask<bool> IsScrollBottom(this IJSRuntime js)
    {
        return js.InvokeAsync<bool>("window.IsScrollBottom");
    }

    public static ValueTask OpenUrlInTab(this IJSRuntime js, string url)
    {
        return js.InvokeVoidAsync("window.open", url, "_blank");
    }

    public static ValueTask CopyToClipboard(this IJSRuntime js, string text)
    {
        return js.InvokeVoidAsync("navigator.clipboard.writeText", text);
    }

    public static ValueTask ScrollToBottom(this IJSRuntime js, string id)
    {
        return js.InvokeVoidAsync("window.ScrollToBottom", id);
    }

    public static ValueTask ScrollToTop(this IJSRuntime js, string id)
    {
        return js.InvokeVoidAsync("window.ScrollToTop", id);
    }

    public static async Task SetTheme(this IJSRuntime js, string theme)
    {
        await js.InvokeVoidAsync("window.setTheme", theme);
    }

    public static async Task<string> GetTheme(this IJSRuntime js)
    {
        return await js.InvokeAsync<string>("window.getTheme");
    }
}
