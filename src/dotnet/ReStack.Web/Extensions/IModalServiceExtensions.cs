using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
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
        var options = new ModalOptions()
        {
            HideHeader = true,
            Size = ModalSize.Medium,
            Position = ModalPosition.TopCenter,
            AnimationType = ModalAnimationType.PopIn,
            Class = "bg-gray-100 dark:bg-neutral-900 p-6 max-w-lg m-auto mt-4 md:mt-10 rounded dark:text-gray-100"
        };
        var modal = modalService.Show<Question>(string.Empty, parameters, options);
        var result = await modal.Result;

        if (!result.Cancelled && result.Data is QuestionResult r)
        {
            return r;
        }

        return QuestionResult.Cancel;
    }

    public static IModalReference AddLibrary(this IModalService modal, string source = null, Dictionary<string, List<string>> validations = null)
    {
        var parameters = new ModalParameters
        {
            { "Source", source ?? string.Empty },
            { "Validations", validations ?? new() }
        };
        var options = new ModalOptions()
        {
            HideHeader = true,
            Size = ModalSize.Medium,
            Position = ModalPosition.TopCenter,
            AnimationType = ModalAnimationType.PopIn,
            Class = "bg-gray-100 dark:bg-neutral-900 p-6 max-w-lg m-auto mt-4 md:mt-10 rounded dark:text-gray-100"
        };
        var result = modal.Show<AddLibrary>(string.Empty, parameters, options);

        return result;
    }

    public static IModalReference Error(this IModalService modal, MarkupString issue, MarkupString cause, Exception exception, Dictionary<string, Task> actions, bool unhandled)
    {
        var parameters = new ModalParameters
        {
            { "Issue", issue },
            { "Cause", cause },
            { "Exception", exception },
            { "Actions", actions },
            { "Unhandled", unhandled }
        };
        var options = new ModalOptions()
        {
            HideHeader = true,
            Size = ModalSize.Medium,
            Position = ModalPosition.TopCenter,
            AnimationType = ModalAnimationType.PopIn,
            Class = "bg-gray-100 dark:bg-neutral-900 p-6 max-w-lg m-auto mt-4 md:mt-10 rounded dark:text-gray-100"
        };
        var result = modal.Show<Error>(string.Empty, parameters, options);

        return result;
    }

    public static IModalReference AddTag(this IModalService modal)
    {
        var parameters = new ModalParameters
        {
        };
        var options = new ModalOptions()
        {
            HideHeader = true,
            Size = ModalSize.Medium,
            Position = ModalPosition.TopCenter,
            AnimationType = ModalAnimationType.PopIn,
            Class = "bg-gray-100 dark:bg-neutral-900 p-6 max-w-lg m-auto mt-4 md:mt-10 rounded dark:text-gray-100"
        };
        var result = modal.Show<AddTag>(string.Empty, parameters, options);

        return result;
    }
}
