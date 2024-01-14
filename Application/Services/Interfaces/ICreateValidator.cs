using FluentValidation.Results;

namespace Application.Services.Interfaces;
public interface ICreateValidator<in TCreateDto>
{
    ValidationResult Validate(TCreateDto dto);
    Task<ValidationResult> ValidateAsync(TCreateDto dto);
}
