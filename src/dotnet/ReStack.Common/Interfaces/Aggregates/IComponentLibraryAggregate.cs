using ReStack.Common.Models;

namespace ReStack.Common.Interfaces.Aggregates;

public interface IComponentLibraryAggregate : IBaseAggregate<ComponentLibraryModel>
{
    Task<ComponentLibraryModel> Sync(string source);
    Task<ComponentLibraryModel> Compose(string source);
    Task<List<StackModel>> GetUsingStacks(int componentLibraryId);
}
