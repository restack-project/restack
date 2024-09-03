
namespace ReStack.Web.Components;

public partial class Breadcrumb
{
    public Dictionary<string, string> Links { get; set; } = [];

    public async Task Add(string name, string value)
    {
        Links.Add(name, value);

        await InvokeAsync(StateHasChanged);
    }

    public async Task Set(Dictionary<string, string> links)
    {
        Links = links;

        await InvokeAsync(StateHasChanged);
    }

    public async Task Remove(string key)
    {
        Links.Remove(key);

        await InvokeAsync(StateHasChanged);
    }
}
