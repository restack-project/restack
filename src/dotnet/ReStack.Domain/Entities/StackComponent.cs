namespace ReStack.Domain.Entities;

public class StackComponent
{
    public int StackId { get; set; }
    public Stack Stack { get; set; }

    public int ComponentId { get; set; }
    public Component Component { get; set; }
}
