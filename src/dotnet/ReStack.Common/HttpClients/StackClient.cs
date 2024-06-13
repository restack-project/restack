using ReStack.Common.Constants;
using ReStack.Common.HttpClients.Base;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;

namespace ReStack.Common.HttpClients;

public class StackClient(
    HttpClient client
) : BaseClient<StackModel>(client, EndPoints.Stack), IStackClient
{
    public Task<JobModel> Execute(int stackId)
        => MakeRequest<JobModel>(HttpMethod.Get, EndPoints.Stack_Execute.Resolve("stackId", stackId));

    public Task<List<StackModel>> GetAll(int numberOfJobs, CancellationToken token = default)
        => MakeRequest<List<StackModel>>(HttpMethod.Get, EndPoints.Stack_GetAll.AddQueryString($"numberOfJobs={numberOfJobs}"), cancellationToken: token);

    public Task<JobModel> Cancel(int stackId)
        => MakeRequest<JobModel>(HttpMethod.Get, EndPoints.Stack_Cancel.Resolve("stackId", stackId));

    public Task<string> DownloadFile(int stackId, CancellationToken token = default)
        => MakeRequest<string>(HttpMethod.Get, EndPoints.Stack_ReadFile.Resolve("stackId", stackId), cancellationToken: token);

    public Task<StackModel> UploadFile(int stackId, string text)
        => MakeRequest<StackModel>(HttpMethod.Post, EndPoints.Stack_UploadFile.Resolve("stackId", stackId), body: text, mediaType: "text/plain");
}
