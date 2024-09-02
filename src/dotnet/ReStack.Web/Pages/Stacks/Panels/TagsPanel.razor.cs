using Microsoft.AspNetCore.Components;
using ReStack.Common.Models;

namespace ReStack.Web.Pages.Stacks.Panels;

public partial class TagsPanel
{
    private bool _;

    [Parameter] public StackModel Stack { get; set; }
    [Parameter] public List<TagModel> Tags { get; set; }

    private async Task TagToggle(bool value, TagModel tag)
    {
        if (value)
        {
            Stack.Tags.Add(tag);
        }
        else
        {
            Stack.Tags.Remove(tag);
        }

        await StateHasChangedAsync();
    }
}
