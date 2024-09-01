using Microsoft.AspNetCore.Mvc;
using ReStack.Api.Extensions;
using ReStack.Common.Constants;
using ReStack.Common.Interfaces.Aggregates;
using ReStack.Common.Models;

namespace ReStack.Api.Controllers;

[ApiController]
public class TagController(
    ITagAggregate _tagAggregate
) : ControllerBase
{
    [HttpGet(EndPoints.Tag_GetAll)]
    public Task<ActionResult<List<TagModel>>> GetAll()
        => _tagAggregate.GetAll().Handle(this);

    [HttpGet(EndPoints.Tag_Get)]
    public Task<ActionResult<TagModel>> Get([FromRoute] int tagId)
        => _tagAggregate.Get(tagId).Handle(this);

    [HttpPost(EndPoints.Tag_Add)]
    public Task<ActionResult<TagModel>> Add(TagModel tag)
        => _tagAggregate.Add(tag).Handle(this);

    [HttpPut(EndPoints.Tag_Update)]
    public Task<ActionResult<TagModel>> Update(TagModel tag)
        => _tagAggregate.Update(tag).Handle(this);

    [HttpDelete(EndPoints.Tag_Delete)]
    public Task<ActionResult<TagModel>> Delete([FromRoute] int tagId)
        => _tagAggregate.Delete(tagId).Handle(this);
}
