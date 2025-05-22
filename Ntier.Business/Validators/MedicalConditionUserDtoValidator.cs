using FluentValidation;
using Ntier.Shared.Dtos;

namespace Ntier.Business.Validators;

public class MedicalConditionUserDtoValidator : AbstractValidator<MedicalConditionUserDto>
{
    public MedicalConditionUserDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.MedicalConditionIds)
            .NotEmpty()
            .WithMessage("MedicalConditionIds is required.")
            .Must(ids => ids.Length > 0)
            .WithMessage("At least one MedicalConditionId is required.")
            .Must(ids => ids.All(id => id > 0))
            .WithMessage("All MedicalConditionIds must be positive integers.");
    }
}