using Microsoft.AspNetCore.Components;
using ReStack.Common.Models;

namespace ReStack.Web.Components;

public partial class JobStateLabel
{
    [Parameter] public JobModel Job { get; set; }
    [Parameter] public bool ShowLabel { get; set; }
}
