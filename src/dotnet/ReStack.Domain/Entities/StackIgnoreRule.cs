namespace ReStack.Domain.Entities;

public class StackIgnoreRule
{
    public int Id { get; set; }
    public bool Enabled { get; set; }
    public string Value { get; set; }

    public int StackId { get; set; }
    public Stack Stack { get; set; }
}
