using Microsoft.AspNetCore.Components;
using ReStack.Common.Interfaces.Clients;

namespace ReStack.Web.Shared;

public partial class MainLayout
{
    private bool _hideAlert = true;

    [Inject] public INotificationClient NotificationClient { get; set; }

    public bool ShowAlert { get => NotificationClient.State != NotificationState.Connected && !_hideAlert; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await NotificationClient.Connect(() => InvokeAsync(StateHasChanged));
            _hideAlert = false;
        }
        catch { }

        await base.OnInitializedAsync();
    }

    private void HideAlert()
    {
        _hideAlert = true;
    }
}
