using Microsoft.AspNetCore.Components;
using ReStack.Common.Models;

namespace ReStack.Web.Pages.Stacks.Panels;

public partial class IgnoreRulesPanel
{
    [Parameter] public StackModel Stack { get; set; }

    private async Task Add()
    {
        Stack.IgnoreRules.Add(new()
        {
            StackId = Stack.Id,
            Enabled = true,
        });

        await Task.CompletedTask;
    }

    private async Task Delete(StackIgnoreRuleModel ignoreRule)
    {
        Stack.IgnoreRules.Remove(ignoreRule);

        await Task.CompletedTask;
    }
}
