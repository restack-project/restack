
using Microsoft.AspNetCore.Components;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;
using ReStack.Web.Extensions;

namespace ReStack.Web.Pages.Libraries;

public partial class Overview
{
    [Inject] public IComponentLibraryClient ComponentLibraryClient { get; set; }

    public List<ComponentLibraryModel> Libraries { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await SetLoading(true);

            BreadcrumbLinks = new()
            {
                { "Libraries", "/libraries" }
            };

            Libraries = await ComponentLibraryClient.GetAll();
        }
        catch (Exception ex)
        {
            await ShowError(ex);
        }
        finally
        {
            await SetLoading(false);
        }

        await base.OnInitializedAsync();
    }

    private async Task Add()
    {
        try
        {
            var result = await Modal.AddLibrary();

            if (result is not null)
            {
                if (!Libraries.Any(x => x.Id == result.Id))
                {
                    Libraries.Add(result);
                }
            }
        }
        catch (Exception ex)
        {
            await ShowError(ex);
        }
    }
}
