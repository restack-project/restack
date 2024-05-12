using ReStack.Common.Constants;
using ReStack.Common.HttpClients.Base;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;

namespace ReStack.Common.HttpClients;

public class ComponentLibraryClient(
    HttpClient client
) : BaseClient<ComponentLibraryModel>(client, EndPoints.ComponentLibrary), IComponentLibraryClient
{
    public Task<ComponentLibraryModel> Sync(string source)
        => MakeRequest<ComponentLibraryModel>(HttpMethod.Post, EndPoints.ComponentLibrary_Sync, new ComponentLibraryModel() { Source = source });

    public Task<List<StackModel>> GetUsingStacks(int componentLibraryId)
        => MakeRequest<List<StackModel>>(HttpMethod.Get, EndPoints.ComponentLibrary_GetUsingStacks.Resolve("componentLibraryId", componentLibraryId));

    public override Task<ComponentLibraryModel> Get(int id, CancellationToken cancellationToken = default)
        => MakeRequest<ComponentLibraryModel>(HttpMethod.Get, EndPoints.ComponentLibrary_Get.Resolve("componentLibraryId", id));

    #region NotImplemented

    public Task<ComponentLibraryModel> Compose(string source)
    {
        throw new NotImplementedException();
    }

    public override Task<ComponentLibraryModel> Add(ComponentLibraryModel model)
    {
        throw new NotImplementedException();
    }

    public override Task<ComponentLibraryModel> Update(ComponentLibraryModel model)
    {
        throw new NotImplementedException();
    }

    #endregion
}
