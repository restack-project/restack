using Microsoft.AspNetCore.Mvc;
using ReStack.Api.Extensions;
using ReStack.Common.Constants;
using ReStack.Common.Interfaces.Aggregates;
using ReStack.Common.Models;

namespace ReStack.Api.Controllers;

[ApiController]
public class SshKeyController(
    ISshKeyAggregate _sshAggregate
) : ControllerBase
{
    [HttpGet(EndPoints.SshKey_Get)]
    public Task<ActionResult<SshKeyModel>> Get()
        => _sshAggregate.Get().Handle(this);

    [HttpGet(EndPoints.SshKey_Generate)]
    public Task<ActionResult<SshKeyModel>> Generate()
        => _sshAggregate.Generate().Handle(this);
}
