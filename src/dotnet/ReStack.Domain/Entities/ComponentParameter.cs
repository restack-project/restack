using ReStack.Domain.ValueObjects;

namespace ReStack.Domain.Entities;

public class ComponentParameter
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DataType DataType { get; set; }

    public int ComponentId { get; set; }
    public Component Component { get; set; }
}
