using Microsoft.AspNetCore.Components;
using ReStack.Common.Models;

namespace ReStack.Web.Pages.Stacks.Panels;

public partial class SettingsPanel
{
    [Parameter] public StackModel Stack { get; set; }
    [Parameter] public List<string> Validations { get; set; } // TODO check UI

    public override string Url => $"stacks/{Stack.Id}/edit/settings";
}

