using FluentValidation;
using ReStack.Common.Interfaces;
using ReStack.Common.Models;

namespace ReStack.Common.Validators;

public class TagModelValidator : AbstractValidator<TagModel>
{
    private readonly IReStackDbContext _context;

    public TagModelValidator(IReStackDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).Must((x, y) => !HasDuplicateName(x)).WithMessage(x => "Name already exists.");
    }

    private bool HasDuplicateName(TagModel model)
    {
        return _context.Tag.Where(x => x.Id != model.Id).Any(x => x.Name.ToLower() == model.Name.ToLower());
    }
}
