using FluentValidation;
using Ntier.Shared.Dtos;

namespace Ntier.Business.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
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
}