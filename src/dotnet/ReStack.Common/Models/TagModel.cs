using ReStack.Domain.Entities;

namespace ReStack.Common.Models;

public class TagModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string HexColor { get; set; }

    public TagModel(StackTag stackTag)
    {
        if (stackTag.Tag is not null)
        {
            Id = stackTag.Tag.Id;
            Name = stackTag.Tag.Name;
            HexColor = stackTag.Tag.HexColor;
        }
    }
}
