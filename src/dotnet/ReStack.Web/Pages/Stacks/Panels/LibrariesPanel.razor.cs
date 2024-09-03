using Microsoft.AspNetCore.Components;
using ReStack.Common.Models;

namespace ReStack.Web.Pages.Stacks.Panels;

public partial class LibrariesPanel
{
    [Parameter] public StackModel Stack { get; set; }
    [Parameter] public List<ComponentLibraryModel> Libraries { get; set; }
    
    public override string Url => $"stacks/{Stack.Id}/edit/tags";
}
