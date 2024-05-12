using Microsoft.AspNetCore.Mvc;
using ReStack.Api.Extensions;
using ReStack.Common.Constants;
using ReStack.Common.Interfaces.Aggregates;
using ReStack.Common.Models;

namespace ReStack.Api.Controllers;

[ApiController]
public class ComponentLibraryController(
    IComponentLibraryAggregate _componentLibraryAggregate
) : ControllerBase
{
    [HttpGet(EndPoints.ComponentLibrary_GetAll)]
    public Task<ActionResult<List<ComponentLibraryModel>>> GetAll()
        => _componentLibraryAggregate.GetAll().Handle(this);

    [HttpGet(EndPoints.ComponentLibrary_Get)]
    public Task<ActionResult<ComponentLibraryModel>> Get([FromRoute] int componentLibraryId)
        => _componentLibraryAggregate.Get(componentLibraryId).Handle(this);

    [HttpGet(EndPoints.ComponentLibrary_GetUsingStacks)]
    public Task<ActionResult<List<StackModel>>> GetUsingStacks([FromRoute] int componentLibraryId)
        => _componentLibraryAggregate.GetUsingStacks(componentLibraryId).Handle(this);

    [HttpPost(EndPoints.ComponentLibrary_Sync)]
    public Task<ActionResult<ComponentLibraryModel>> Sync(ComponentLibraryModel model)
        => _componentLibraryAggregate.Sync(model.Source).Handle(this);

    [HttpPost(EndPoints.ComponentLibrary_Compose)]
    public Task<ActionResult<ComponentLibraryModel>> Update(ComponentLibraryModel model)
        => _componentLibraryAggregate.Compose(model.Source).Handle(this);

    [HttpDelete(EndPoints.ComponentLibrary_Delete)]
    public Task<ActionResult<ComponentLibraryModel>> Delete([FromRoute] int componentLibraryId)
        => _componentLibraryAggregate.Delete(componentLibraryId).Handle(this);
}
