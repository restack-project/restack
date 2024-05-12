using FluentValidation;
using ReStack.Common.Models;

namespace ReStack.Common.Validators;

public class PipAggregateValidator : AbstractValidator<PythonPackgeModel>
{
    public PipAggregateValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
