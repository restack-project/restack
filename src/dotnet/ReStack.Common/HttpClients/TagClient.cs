using ReStack.Common.Constants;
using ReStack.Common.HttpClients.Base;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;

namespace ReStack.Common.HttpClients;

public class TagClient(
    HttpClient _client
) : BaseClient<TagModel>(_client, EndPoints.Tag), ITagClient
{
}
