using FluentValidation;
using Ntier.Shared.Dtos;

namespace Ntier.Business.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.Password).NotEmpty();
    }
}