using ReStack.Domain.Entities;

namespace ReStack.Application.StackHandling.Validators;

public class IsLinuxValidator : BaseStrategyValidator
{
    public override Task<List<Log>> Validate(Stack stack)
    {
        if (!OperatingSystem.IsLinux())
            Logs.Add(Log.CreateError($"Stack can only run on linux"));

        return Task.FromResult(Logs);
    }
}
