namespace ReStack.Common.Models;

public class ComponentParameterModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DataType { get; set; }

    public int ComponentId { get; set; }
}
