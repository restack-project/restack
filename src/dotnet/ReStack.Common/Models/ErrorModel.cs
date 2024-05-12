namespace ReStack.Common.Models;

public enum ErrorModelType
{
    StackFileNotFound
}

public class ErrorModel
{
    public ErrorModelType Type { get; set; }

    public ErrorModel(ErrorModelType type)
    {
        Type = type;
    }
}
