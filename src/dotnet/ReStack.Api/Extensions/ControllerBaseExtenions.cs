using Microsoft.AspNetCore.Mvc;
using ReStack.Common.Exceptions;

namespace ReStack.Api.Extensions;

public static class ControllerBaseExtenions
{
    public static async Task<ActionResult<TE>> Handle<TE>(this Task<TE> method, ControllerBase controller)
    {
        try
        {
            var result = await method;

            return controller.Ok(result);
        }
        catch (LibraryComposeException e)
        {
            return controller.BadRequest(new ModelValidationBadRequest("Error while syncing library", controller.HttpContext.TraceIdentifier, e.Validations));
        }
        catch (ModelValidationException e)
        {
            return controller.BadRequest(e.Create(controller.HttpContext.TraceIdentifier));
        }
    }
}