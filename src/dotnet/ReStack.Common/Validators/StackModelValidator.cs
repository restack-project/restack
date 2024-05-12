using FluentValidation;
using ReStack.Common.Interfaces;
using ReStack.Common.Models;

namespace ReStack.Common.Validators;

public class StackModelValidator : AbstractValidator<StackModel> 
{
    private readonly IReStackDbContext _context;

    public StackModelValidator(IReStackDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).Must((x, y) => !HasDuplicateName(x)).WithMessage(x => "Name must be unique");
    }

    private bool HasDuplicateName(StackModel stack)
    {
        return _context.Stack.Where(x => x.Id != stack.Id).Any(x => x.Name.ToLower() == stack.Name.ToLower());
    }
}
