using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ReStack.Web.Extensions;

namespace ReStack.Web.Shared;

public partial class Page
{
    private readonly string _contentId = Guid.NewGuid().ToString();
    private readonly string _collapsedSidePanelKey = "SidePanel_Collapsed";

    [Inject] public ILocalStorageService LocalStorage { get; set; }
    [Inject] public IJSRuntime JS { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public new Dictionary<string, string> BreadcrumbLinks { get; set; } = [];

    public bool CollapsedSidePanel { get; set; } = true;
    public bool EnabledDarkMode { get; set; }

    public string CssSidePanel { get => CollapsedSidePanel ? "w-[3rem]" : "w-[18rem]"; }
    public string CssSidePanelItem { get => CollapsedSidePanel ? "p-6" : "p-4"; }

    public async Task ScrollToBottom()
    {
        await JS.ScrollToBottom(_contentId);
    }

    public async Task ScrollToTop()
    {
        await JS.ScrollToTop(_contentId);
    }

    protected override async Task OnInitializedAsync()
    {
        CollapsedSidePanel = await LocalStorage.GetItemAsync<bool>(_collapsedSidePanelKey);

        var theme = await JS.GetTheme();

        EnabledDarkMode = theme == "dark";

        await base.OnInitializedAsync();
    }

    private async Task ToggleDarkMode()
    {
        EnabledDarkMode = !EnabledDarkMode;

        if (EnabledDarkMode)
        {
            await JS.SetTheme("dark");
        }
        else
        {
            await JS.SetTheme("light");
        }
    }

    private async Task ToggleCollapsedSidePanel()
    {
        CollapsedSidePanel = !CollapsedSidePanel;

        await LocalStorage.SetItemAsync<bool>(_collapsedSidePanelKey, CollapsedSidePanel);

        await StateHasChangedAsync();
    }
}
