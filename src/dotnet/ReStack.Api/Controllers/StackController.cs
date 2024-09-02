using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReStack.Api.Extensions;
using ReStack.Application.Aggregates;
using ReStack.Common.Constants;
using ReStack.Common.Interfaces.Aggregates;
using ReStack.Common.Models;
using System.Text;

namespace ReStack.Api.Controllers;

[ApiController]
[Authorize]
public class StackController(
    IStackAggregate _stackAggregate
) : ControllerBase
{
    [HttpGet(EndPoints.Stack_GetAll)]
    public Task<ActionResult<List<StackModel>>> GetAll([FromQuery] int numberOfJobs = 1)
        => _stackAggregate.GetAll(numberOfJobs).Handle(this);

    [HttpGet(EndPoints.Stack_Get)]
    public Task<ActionResult<StackModel>> Get([FromRoute] int stackId)
        => _stackAggregate.Get(stackId).Handle(this);

    [HttpPost(EndPoints.Stack_Add)]
    public Task<ActionResult<StackModel>> Add(StackModel stack)
        => _stackAggregate.Add(stack).Handle(this);

    [HttpPut(EndPoints.Stack_Update)]
    public Task<ActionResult<StackModel>> Update(StackModel stack)
        => _stackAggregate.Update(stack).Handle(this);

    [HttpDelete(EndPoints.Stack_Delete)]
    public Task<ActionResult<StackModel>> Delete([FromRoute] int stackId)
        => _stackAggregate.Delete(stackId).Handle(this);

    [HttpGet(EndPoints.Stack_Execute)]
    public Task<ActionResult<JobModel>> Execute([FromRoute] int stackId)
        => _stackAggregate.Execute(stackId).Handle(this);

    [HttpGet(EndPoints.Stack_Cancel)]
    public Task<ActionResult<JobModel>> Cancel([FromRoute] int stackId)
        => _stackAggregate.Cancel(stackId).Handle(this);

    [HttpGet(EndPoints.Stack_ReadFile)]
    public async Task<ActionResult<string>> ReadFile([FromRoute] int stackId)
    {
        try
        {
            return await _stackAggregate.DownloadFile(stackId).Handle(this);
        }
        catch (Exception ex) when (ex is DirectoryNotFoundException || ex is FileNotFoundException)
        {
            return StatusCode(500, new ErrorModel(ErrorModelType.StackFileNotFound));
        }
    }

    [HttpPost(EndPoints.Stack_UploadFile)]
    public async Task<ActionResult<StackModel>> UploadFile([FromRoute] int stackId)
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        var text = await reader.ReadToEndAsync();

        return await _stackAggregate.UploadFile(stackId, text).Handle(this);
    }
}
