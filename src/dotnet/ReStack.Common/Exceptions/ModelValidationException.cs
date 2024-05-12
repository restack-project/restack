using FluentValidation.Results;
using System.Diagnostics;

namespace ReStack.Common.Exceptions;

public class ModelValidationException : Exception
{
    public Dictionary<string, List<string>> Validations { get; set; } = new();

    public ModelValidationException(ValidationResult validationResult) : base($"ValidationError")
    {
        foreach (var error in validationResult.Errors)
        {
            if (!Validations.ContainsKey(error.PropertyName))
                Validations.Add(error.PropertyName, new());

            Validations[error.PropertyName].Add(error.ErrorMessage);
        }
    }

    public ModelValidationException(string property, string validation)
    {
        Validations = new()
        {
            { property, new() { validation } }
        };
    }

    public ModelValidationException(Dictionary<string, List<string>> validations)
    {
        Validations = validations;
    }

    public ModelValidationBadRequest Create(string traceId) => new(Message, traceId, Validations);
}

public class ModelValidationBadRequest
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; } = 400;
    public string TraceId { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; }

    public ModelValidationBadRequest()
    {

    }

    public ModelValidationBadRequest(string title, string traceId, Dictionary<string, List<string>> error)
    {
        Title = title;
        TraceId = Activity.Current?.Id ?? traceId;
        Errors = error;
    }
}
