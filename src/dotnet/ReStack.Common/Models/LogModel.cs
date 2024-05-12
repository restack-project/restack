namespace ReStack.Common.Models;

public class LogModel
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
    public bool Error { get; set; }

    public int JobId { get; set; }
}