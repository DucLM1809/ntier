using FluentValidation;
using Ntier.Shared.Dtos;

namespace Ntier.Business.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
            .WithMessage(
                "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number and one special character"
            );

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(role => role == "User" || role == "Admin")
            .WithMessage("Role must be either 'User' or 'Admin'");
    }
}
