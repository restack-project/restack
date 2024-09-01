namespace ReStack.Domain.Entities;

public class StackTag
{
    public int StackId { get; set; }
    public Stack Stack { get; set; }

    public int TagId { get; set; }
    public Tag Tag { get; set; }
}
