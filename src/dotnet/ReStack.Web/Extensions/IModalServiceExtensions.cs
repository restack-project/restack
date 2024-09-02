using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ReStack.Common.Models;
using ReStack.Web.Modals;

namespace ReStack.Web.Extensions;

public static class IModalServiceExtensions
{
    public static Task<QuestionResult> Question(this IModalService modal, string title)
    {
        return modal.Question(title, string.Empty);
    }

    public static async Task<QuestionResult> Question(this IModalService modalService, string title, string body)
    {
        var parameters = new ModalParameters
        {
            { "Title", title },
            { "Body", body }
        };
        var result = await ShowModal<Question>(modalService, parameters);

        if (!result.Cancelled && result.Data is QuestionResult r)
        {
            return r;
        }

        return QuestionResult.Cancel;
    }

    public static async Task<TagModel> AddTag(this IModalService modal, TagModel tag = null)
    {
        var parameters = new ModalParameters
        {
            { "Tag", tag }
        };

        var result = await ShowModal<AddTag>(modal, parameters);

        if (!result.Cancelled && result.Data is TagModel resp)
        {
            return resp;
        }

        return null;
    }

    public static async Task<ComponentLibraryModel> AddLibrary(this IModalService modal, string source = null, Dictionary<string, List<string>> validations = null)
    {
        var parameters = new ModalParameters
        {
            { "Source", source ?? string.Empty },
            { "Validations", validations ?? new() }
        };

        var result = await ShowModal<AddLibrary>(modal, parameters);

        if (!result.Cancelled && result.Data is ComponentLibraryModel library)
        {
            return library;
        }

        return null;
    }

    public static Task<ModalResult> Error(this IModalService modal, MarkupString issue, MarkupString cause, Exception exception, Dictionary<string, Task> actions, bool unhandled)
    {
        var parameters = new ModalParameters
        {
            { "Issue", issue },
            { "Cause", cause },
            { "Exception", exception },
            { "Actions", actions },
            { "Unhandled", unhandled }
        };

        return ShowModal<Error>(modal, parameters);
    }

    private static async Task<ModalResult> ShowModal<TComponent>(IModalService modalService, ModalParameters parameters = null) where TComponent : IComponent
    {
        var options = new ModalOptions()
        {
            HideHeader = true,
            Size = ModalSize.Medium,
            Position = ModalPosition.TopCenter,
            AnimationType = ModalAnimationType.PopIn,
            Class = "bg-gray-100 dark:bg-neutral-900 p-6 max-w-lg m-auto mt-4 md:mt-10 rounded dark:text-gray-100"
        };
        var modal = modalService.Show<TComponent>(string.Empty, parameters ?? [], options);
        var result = await modal.Result;

        return result;
    }
}
