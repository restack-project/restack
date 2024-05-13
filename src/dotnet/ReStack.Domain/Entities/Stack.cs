using ReStack.Domain.Settings;
using ReStack.Domain.ValueObjects;

namespace ReStack.Domain.Entities;

public class Stack
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ProgrammingLanguage Type { get; set; }
    public bool FailOnStdError { get; set; }

    public decimal AverageRuntime { get; set; }
    public decimal SuccesPercentage { get; set; }

    public List<Job> Jobs { get; set; }
    public List<StackComponent> Components { get; set; } = [];

    // TODO place this in a service or something else
    public string GetLocation(ApiSettings settings) => Path.Combine(settings.StackStorage, Id.ToString(), DetermineFileName());
    public string GetFileName() => DetermineFileName();

    private string DetermineFileName()
    {
        return $"run{GetExtension()}";
    }

    // todo move to extension method
    public string GetExtension()
    {
        return Type switch
        {
            ProgrammingLanguage.Bat => ".bat",
            ProgrammingLanguage.Shell => ".sh",
            ProgrammingLanguage.PowerShell => ".ps1",
            _ => throw new ArgumentException($"{Type} has no extension configured, please add one."),
        };
    }
}
