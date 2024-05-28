namespace ReStack.Common.Models;

public class StackIgnoreRuleModel
{
    public int Id { get; set; }
    public bool Enabled { get; set; }
    public string Value { get; set; }
    public int StackId { get; set; }
}
