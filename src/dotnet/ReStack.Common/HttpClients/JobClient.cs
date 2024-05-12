using ReStack.Common.Constants;
using ReStack.Common.HttpClients.Base;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;

namespace ReStack.Common.HttpClients;

public class JobClient(HttpClient client) : BaseClient<JobModel>(client, EndPoints.Job), IJobClient
{
    public Task<JobModel> Cancel(int jobId)
        => MakeRequest<JobModel>(HttpMethod.Get, EndPoints.Job_Cancel.Resolve("jobId", jobId));

    public Task<JobModel> GetBySequence(int stackId, int sequence, CancellationToken token = default)
        => MakeRequest<JobModel>(HttpMethod.Get, EndPoints.Job_GetBySequence.Resolve("sequence", sequence).Resolve("stackId", stackId), cancellationToken: token);

    public Task<List<JobModel>> Take(int stackId, int skip, int take, CancellationToken token = default)
        => MakeRequest<List<JobModel>>(HttpMethod.Get, EndPoints.Job_GetAllSkip.Resolve("stackId", stackId).Resolve("skip", skip).Resolve("take", take), cancellationToken: token);

    #region NotImplemented

    public override Task<JobModel> Add(JobModel model)
    {
        throw new NotImplementedException();
    }

    public Task<List<JobModel>> Get(int stackId, DateTime? dateTime)
    {
        throw new NotImplementedException();
    }

    public override Task<List<JobModel>> GetAll(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<JobModel> Update(JobModel model)
    {
        throw new NotImplementedException();
    }

    public Task<JobModel> GetLatest(int stackId)
    {
        throw new NotImplementedException();
    }

    #endregion
}
