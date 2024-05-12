namespace ReStack.Common.Models;

public class ComponentModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FileName { get; set; }
    public string FolderName { get; set; }
    public bool InUse { get; set; }

    public int ComponentLibraryId { get; set; }
    public ComponentLibraryModel Library { get; set; }
    public List<ComponentParameterModel> Parameters { get; set; }

    public string WorkspaceFolder { get; set; }
    public string WorkspaceFile { get; set; }
}
