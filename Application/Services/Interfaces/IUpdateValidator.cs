using FluentValidation.Results;

namespace Application.Services.Interfaces;
public interface IUpdateValidator<in TUpdateDto>
{
    ValidationResult Validate(TUpdateDto dto);
    Task<ValidationResult> ValidateAsync(TUpdateDto dto);
}
