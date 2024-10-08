﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ReStack.Common.Exceptions;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;

namespace ReStack.Web.Modals;

public partial class AddTag
{
    private ValidationMessageStore _store;

    [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

    [Inject] public ITagClient TagClient { get; set; }

    [Parameter] public Dictionary<string, List<string>> Validations { get; set; } = [];
    [Parameter] public TagModel Tag { get; set; }

    public EditContext EditContext { get; set; }
    public bool IsNew { get; set; }

    protected override Task OnInitializedAsync()
    {
        IsNew = Tag is null;

        if (IsNew)
        {
            Tag = new()
            {
                HexColor = string.Format("#{0:X6}", new Random().Next(0x1000000))
            };
        }

        EditContext = new(Tag);
        _store = new(EditContext);

        return base.OnInitializedAsync();
    }

    private async Task Save()
    {
        try
        {
            Validations = [];

            await SetLoading(true);

            if (await Validate())
            {
                if (IsNew)
                {
                    Tag = await TagClient.Add(Tag);
                }
                else
                {
                    Tag = await TagClient.Update(Tag);
                }

                await BlazoredModal.CloseAsync(ModalResult.Ok(Tag));
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

        if (string.IsNullOrWhiteSpace(Tag.Name))
        {
            _store.Add(() => Tag.Name, $"can not be empty");
        }


        if (string.IsNullOrWhiteSpace(Tag.HexColor))
        {
            _store.Add(() => Tag.HexColor, $"can not be empty");
        }

        return Task.FromResult(EditContext.Validate());
    }
}
