using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ntier.Business.Validators.Helpers;
using Ntier.DataAccess;
using Ntier.Shared.Dtos;
using Ntier.Shared.Enums;

namespace Ntier.Business.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    private readonly DataContext _context;

    public UserDtoValidator(DataContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync((email, cancellationToken) => UserValidatorHelpers.BeUniqueEmail(_context, email, cancellationToken)).WithMessage("Email is already taken.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
            .WithMessage(
                "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number and one special character"
            );

        RuleFor(x => x.Role)
            .Must(role => Enum.IsDefined(typeof(Role), role)) // Check if enum exists
            .WithMessage("Invalid role. Allowed values: 'User', 'Admin'.");
    }
}