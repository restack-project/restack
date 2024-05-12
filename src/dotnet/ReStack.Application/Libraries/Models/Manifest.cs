namespace ReStack.Application.Libraries.Models;

public class Manifest
{
    public string Name { get; set; }
    public string Documentation { get; set; }
    public List<string> CodeOwners { get; set; }
    public string Version { get; set; }
    public string Type { get; set; }
}
