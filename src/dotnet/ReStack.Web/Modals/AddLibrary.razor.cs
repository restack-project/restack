using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ReStack.Common.Exceptions;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;

namespace ReStack.Web.Modals;

public partial class AddLibrary
{
    private ValidationMessageStore _store;

    [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

    [Inject] public IComponentLibraryClient ComponentLibraryClient { get; set; }

    [Parameter] public string Source { get; set; }
    [Parameter] public Dictionary<string, List<string>> Validations { get; set; } = [];

    public bool IsUpdate { get; private set; }
    public EditContext EditContext { get; set; }
    public ComponentLibraryModel Library { get; set; } = new();

    protected override Task OnInitializedAsync()
    {
        Library = new() { Source = Source };
        EditContext = new(Library);
        _store = new(EditContext);

        if (!string.IsNullOrWhiteSpace(Library.Source))
            IsUpdate = true;

        return base.OnInitializedAsync();
    }

    private async Task Add()
    {
        try
        {
            Validations = [];

            await SetLoading(true);

            if (await Validate())
            {
                Library = await ComponentLibraryClient.Sync(Library.Source);
                await BlazoredModal.CloseAsync(ModalResult.Ok(Library));
            }
        }
        catch (ModelValidationException ex)
        {
            Validations = ex.Validations;
        }
        catch (Exception ex)
        {
            await ShowError(ex);
        }
        finally
        {
            await SetLoading(false);
        }
    }

    private async Task Cancel()
    {
        await BlazoredModal.CancelAsync();
    }

    private Task<bool> Validate()
    {
        _store.Clear();

        if (string.IsNullOrWhiteSpace(Library.Source))
        {
            _store.Add(() => Library.Source, $"can not be empty");
            Validations.Add("Source", ["Source can not be empty"]);
        }
        else
        {
            if (!Uri.TryCreate(Library.Source, UriKind.Absolute, out var _))
            {
                _store.Add(() => Library.Source, $"is not a valid url");
                Validations.Add("Source", ["Source is not a valid url"]);
            }
        }

        return Task.FromResult(EditContext.Validate());
    }
}
