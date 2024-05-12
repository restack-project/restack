using ReStack.Domain.ValueObjects;

namespace ReStack.Domain.Entities;

public class Job
{
    public int Id { get; set; }
    public int Sequence { get; set; }
    public DateTime Started { get; set; }
    public DateTime? Ended { get; set; }
    public JobState State { get; set; }
    public string TriggerBy { get; set; }

    public int StackId { get; set; }
    public Stack Stack { get; set; }

    public List<Log> Logs { get; set; } = [];
}
