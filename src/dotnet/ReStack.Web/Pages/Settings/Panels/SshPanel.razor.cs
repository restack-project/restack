﻿using Microsoft.AspNetCore.Components;
using ReStack.Common.Interfaces.Clients;
using ReStack.Web.Components;
using ReStack.Web.Extensions;
using ReStack.Web.Modals;

namespace ReStack.Web.Pages.Settings.Panels;

public partial class SshPanel : Panel
{
    [Inject] public ISshKeyClient SshClient { get; set; }

    public override string Url { get => "settings/ssh"; }

    private async Task<string> GetSshKey()
    {
        var key = string.Empty;

        try
        {
            var keyModel = await SshClient.Get();

            key = keyModel.Key;
        }
        catch (Exception ex)
        {
            await ShowError(ex);
        }

        return key;
    }

    protected async Task Regenerate()
    {
        var answer = await Modal.Question("Regenerate ssh key?", "Be aware, this will delete the existing one.");

        if (answer == QuestionResult.Yes)
        {
            try
            {
                await SetLoading(true);

                await SshClient.Generate();
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
    }
}
