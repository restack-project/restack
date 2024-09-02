using Microsoft.AspNetCore.Components;
using ReStack.Common.Models;

namespace ReStack.Web.Pages.Stacks.Panels;

public partial class LibrariesPanel
{
    [Parameter] public List<ComponentLibraryModel> Libraries { get; set; }
}
