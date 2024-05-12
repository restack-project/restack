using System.Text.Json.Serialization;

namespace ReStack.Common.Models;

public class ComponentLibraryModel : IEquatable<ComponentLibraryModel>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Source { get; set; }
    public string Version { get; set; }
    public string CodeOwners { get; set; }
    public string Documentation { get; set; }
    public string Type { get; set; }
    public string Slug { get; set; }
    public string LastHashCommit { get; set; }

    public List<ComponentModel> Components { get; set; } = [];

    [JsonIgnore] 
    public bool InUse { get; set; }
    [JsonIgnore]
    public string SourceRepository { get => Source.Replace(".git", string.Empty); }


    public bool Equals(ComponentLibraryModel other)
    {
        return other?.Id == Id;
    }
}
