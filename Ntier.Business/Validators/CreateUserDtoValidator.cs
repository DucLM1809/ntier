using FluentValidation;
using Ntier.Business.Validators.Helpers;
using Ntier.DataAccess;
using Ntier.Shared.Dtos;

namespace Ntier.Business.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    private readonly DataContext _dataContext;

    public CreateUserDtoValidator(DataContext dataContext)
    {
        _dataContext = dataContext;

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync((email, cancellationToken) =>
                UserValidatorHelpers.BeUniqueEmail(_dataContext, email, cancellationToken))
            .WithMessage("Email is already taken.");

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