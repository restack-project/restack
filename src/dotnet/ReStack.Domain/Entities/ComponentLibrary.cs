using ReStack.Domain.Settings;
using ReStack.Domain.ValueObjects;

namespace ReStack.Domain.Entities;

public class ComponentLibrary
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Source { get; set; }
    public string Version { get; set; }
    public string CodeOwners { get; set; }
    public string Documentation { get; set; }
    public string Slug { get; set; }
    public string LastHashCommit { get; set; }
    public ProgrammingLanguage Type { get; set; }

    public List<Component> Components { get; set; } = [];

    public string GetLocation(ApiSettings settings) => Path.Combine(settings.ComponentStorage, Id.ToString()); 
}
