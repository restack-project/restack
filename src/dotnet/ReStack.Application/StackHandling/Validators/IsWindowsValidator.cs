using ReStack.Domain.Entities;

namespace ReStack.Application.StackHandling.Validators;

public class IsWindowsValidator : BaseStrategyValidator
{
    public override Task<List<Log>> Validate(Stack stack)
    {
        if (!OperatingSystem.IsWindows())
            Logs.Add(Log.CreateError($"Stack can only run on windows"));

        return Task.FromResult(Logs);
    }
}
