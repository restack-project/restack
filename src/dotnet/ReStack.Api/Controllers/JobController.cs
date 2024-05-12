using Microsoft.AspNetCore.Mvc;
using ReStack.Api.Extensions;
using ReStack.Common.Constants;
using ReStack.Common.Interfaces.Aggregates;
using ReStack.Common.Models;

namespace ReStack.Api.Controllers;

[ApiController]
public class JobController(
    IJobAggregate _jobAggregate
) : ControllerBase
{
    [HttpGet(EndPoints.Job_Get)]
    public Task<ActionResult<JobModel>> Get([FromRoute] int jobId)
        => _jobAggregate.Get(jobId).Handle(this);

    [HttpGet(EndPoints.Job_GetBySequence)]
    public Task<ActionResult<JobModel>> GetBySequence([FromRoute] int sequence, int stackId)
        => _jobAggregate.GetBySequence(stackId, sequence).Handle(this);

    [HttpGet(EndPoints.Job_GetAllSkip)]
    public Task<ActionResult<List<JobModel>>> GetAllSkip([FromRoute] int stackId, [FromRoute] int skip, [FromRoute] int take)
        => _jobAggregate.Take(stackId, skip, take).Handle(this);


    [HttpGet(EndPoints.Job_Cancel)]
    public Task<ActionResult<JobModel>> Cancel([FromRoute] int jobId)
        => _jobAggregate.Cancel(jobId).Handle(this);

    [HttpDelete(EndPoints.Job_Delete)]
    public Task<ActionResult<JobModel>> Delete([FromRoute] int jobId)
        => _jobAggregate.Delete(jobId).Handle(this);
}
