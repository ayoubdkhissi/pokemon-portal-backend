using Application.DTOs.Pokemon;
using Application.Services.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Shared.Constants;

namespace Application.Validators.Pokemon;
public class CreatePokemonDtoValidator : AbstractValidator<PokemonManipulationDto>, ICreateValidator<PokemonManipulationDto>
{
    public CreatePokemonDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithErrorCode(ErrorCodes.PropertyIsRequired)
            .MaximumLength(512).WithErrorCode(ErrorCodes.LengthExceeded);

        RuleFor(x => x.Attack)
            .GreaterThan(0).WithErrorCode(ErrorCodes.NegativeValue);

        RuleFor(x => x.Defense)
            .GreaterThan(0).WithErrorCode(ErrorCodes.NegativeValue);

    }

    public new ValidationResult Validate(PokemonManipulationDto dto)
    {
        return base.Validate(dto);
    }

    public async Task<ValidationResult> ValidateAsync(PokemonManipulationDto dto)
    {
        return await base.ValidateAsync(dto);
    }
}
