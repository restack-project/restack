using FluentValidation;
using ReStack.Common.Models;

namespace ReStack.Common.Validators;

public class TagModelValidator : AbstractValidator<TagModel>
{
    public TagModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
