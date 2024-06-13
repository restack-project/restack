using ReStack.Common.Models;

namespace ReStack.Common.Interfaces.Aggregates;

public interface IStackAggregate : IBaseAggregate<StackModel>
{
    Task<List<StackModel>> GetAll(int numberOfJobs, CancellationToken token = default);
    Task<JobModel> Execute(int stackId);
    Task<JobModel> Cancel(int stackId);
    Task<string> DownloadFile(int stackId, CancellationToken token = default);
    Task<StackModel> UploadFile(int stackId, string text);
}
