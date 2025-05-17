using FluentValidation;
using Ntier.Business.Validators.Helpers;
using Ntier.DataAccess;
using Ntier.Shared.Dtos;
using Ntier.Shared.Enums;

namespace Ntier.Business.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    private readonly DataContext _dataContext;

    public RegisterDtoValidator(DataContext dataContext)
    {
        _dataContext = dataContext;

        RuleFor(x => x.Email)
               .NotEmpty()
               .EmailAddress()
               .MustAsync((email, cancellationToken) => UserValidatorHelpers.BeUniqueEmail(_dataContext, email, cancellationToken))
               .WithMessage("Email is already taken.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
            .WithMessage(
                "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number and one special character"
            );
        ;

        RuleFor(x => x.Gender)
    .Must(gender => Enum.IsDefined(typeof(Gender), gender))
    .WithMessage("Invalid gender. Allowed values: 'Male', 'Female', 'Other'.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage("Name must contain only letters and spaces.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThan(DateTime.Now)
            .WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.Height)
            .NotEmpty()
            .InclusiveBetween(0.5f, 3.0f)
            .WithMessage("Height must be between 0.5 and 3.0 meters.");

        RuleFor(x => x.Avatar)
        .Must(x => x == null || x.StartsWith("http"))
        .WithMessage("Avatar must be a valid URL or null.");
    }
}