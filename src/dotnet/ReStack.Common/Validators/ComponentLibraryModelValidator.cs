using FluentValidation;
using ReStack.Common.Interfaces;
using ReStack.Common.Models;

namespace ReStack.Common.Validators;

public class ComponentLibraryModelValidator : AbstractValidator<ComponentLibraryModel>
{
    private readonly IReStackDbContext _context;

    public ComponentLibraryModelValidator(IReStackDbContext context)
    {
        _context = context;

        RuleFor(x => x.Source).Must((x, y) => !HasDuplicateSource(x)).WithMessage(x => "Source already exists.");
    }

    private bool HasDuplicateSource(ComponentLibraryModel model)
    {
        return _context.ComponentLibrary.Where(x => x.Id != model.Id).Any(x => x.Source.ToLower() == model.Source.ToLower());
    }
}
