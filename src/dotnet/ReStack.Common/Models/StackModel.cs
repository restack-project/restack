using ReStack.Domain.ValueObjects;

namespace ReStack.Common.Models;

public class StackModel : IEquatable<StackModel>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string FileName { get; set; }
    public bool FailOnStdError { get; set; }

    public TimeSpan AverageRuntime { get; set; }
    public decimal SuccesPercentage { get; set; }

    public ICollection<JobModel> Jobs { get; set; } = [];
    public ICollection<ComponentModel> Components { get; set; } = [];
    public ICollection<StackIgnoreRuleModel> IgnoreRules { get; set; } = [];
    public ICollection<TagModel> Tags { get; set; } = [];
    public JobModel LastJob { get => Jobs.OrderByDescending(x => x.Sequence).FirstOrDefault(); }
    public JobModel RunningJob { get => Jobs.Where(x => x.State == JobState.Running.ToString() || x.State == JobState.Queued.ToString()).OrderByDescending(x => x.Sequence).FirstOrDefault(); }

    public bool Equals(StackModel other)
    {
        return other?.Id == Id;
    }
}
