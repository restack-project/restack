using ReStack.Domain.Settings;

namespace ReStack.Domain.Entities;

public class Component
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FileName { get; set; }
    public string FolderName { get; set; }

    public int ComponentLibraryId { get; set; }
    public ComponentLibrary ComponentLibrary { get; set; }

    public ICollection<ComponentParameter> Parameters { get; set; } = [];
    public ICollection<StackComponent> Stacks { get; set; } = [];

    public string GetLocation(ApiSettings settings) => Path.Combine(settings.ComponentStorage, ComponentLibraryId.ToString(), "components", FolderName);
}
