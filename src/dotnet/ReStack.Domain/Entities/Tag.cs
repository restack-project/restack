namespace ReStack.Domain.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string HexColor { get; set; }

    public ICollection<StackTag> Stacks { get; set; } = [];
}
