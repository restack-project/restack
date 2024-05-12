using ReStack.Domain.ValueObjects;

namespace ReStack.Common.Models;

public class JobModel
{
    public int Id { get; set; }
    public int Sequence { get; set; }
    public DateTime Started { get; set; }
    public DateTime? Ended { get; set; }
    public string State { get; set; }
    public string TriggerBy { get; set; }

    public int StackId { get; set; }

    public List<LogModel> Logs { get; set; }

    public TimeSpan Duration { get => Ended.HasValue ? (Ended.Value - Started) : TimeSpan.Zero; }
    public string LabelColor { get => DetermineLabelColor(); }

    private string DetermineLabelColor()
    {
        if (State == JobState.Success.ToString())
            return "bg-success";
        else if (State == JobState.Failed.ToString())
            return "bg-danger";
        else if (State == JobState.Running.ToString())
            return "bg-info";
        else if (State == JobState.Queued.ToString())
            return "bg-warning";
        else
            return "bg-main-200";
    }
}