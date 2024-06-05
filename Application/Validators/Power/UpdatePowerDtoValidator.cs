using Application.DTOs.Power;
using Application.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Shared.Constants;

namespace Application.Validators.Power;
public class UpdatePowerDtoValidator : AbstractValidator<PowerManipulationDto>, IUpdateValidator<PowerManipulationDto>
{
    public UpdatePowerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithErrorCode(ErrorCodes.PropertyIsRequired)
            .MaximumLength(512).WithErrorCode(ErrorCodes.LengthExceeded);

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithErrorCode(ErrorCodes.PropertyIsRequired)
            .MaximumLength(2048).WithErrorCode(ErrorCodes.LengthExceeded);

    }

    public new ValidationResult Validate(PowerManipulationDto dto)
    {
        return base.Validate(dto);
    }

    public async Task<ValidationResult> ValidateAsync(PowerManipulationDto dto)
    {
        return await base.ValidateAsync(dto);
    }
}
