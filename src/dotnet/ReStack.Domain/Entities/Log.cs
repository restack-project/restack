using System.Text.RegularExpressions;

namespace ReStack.Domain.Entities;

public class Log
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
    public bool Error { get; set; }

    public int JobId { get; set; }
    public Job Job { get; set; }

    public static Log CreateError(string message) => new()
    {
        Message = message,
        Error = true,
        Timestamp = DateTime.UtcNow
    };

    public static Log CreateSuccess(string message) => new()
    {
        Message = message,
        Timestamp = DateTime.UtcNow
    };

    public void CheckIgnoreRules(ICollection<StackIgnoreRule> ignoreRules)
    {
        Error = true;

        foreach (var rule in ignoreRules.Where(x => x.Enabled))
        {
            var ignoreError = Regex.Match(Message, rule.Value);

            if (ignoreError.Success)
            {
                Error = false;

                break;
            }
        }
    }
}
