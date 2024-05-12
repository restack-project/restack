using ReStack.Common.Models;

namespace ReStack.Common.Interfaces.Aggregates;

public interface ISshKeyAggregate
{
    Task<SshKeyModel> Get(CancellationToken token = default);
    Task<SshKeyModel> Generate();
}
