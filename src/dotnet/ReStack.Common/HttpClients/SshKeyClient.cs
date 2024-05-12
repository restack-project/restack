using ReStack.Common.Constants;
using ReStack.Common.HttpClients.Base;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;

namespace ReStack.Common.HttpClients;

public class SshKeyClient(
    HttpClient client
) : BaseClient<SshKeyModel>(client, EndPoints.SshKey_Generate), ISshKeyClient
{
    public Task<SshKeyModel> Generate()
        => MakeRequest<SshKeyModel>(HttpMethod.Get, EndPoints.SshKey_Generate);

    public Task<SshKeyModel> Get(CancellationToken token = default)
        => MakeRequest<SshKeyModel>(HttpMethod.Get, EndPoints.SshKey_Get, cancellationToken: token);
}
