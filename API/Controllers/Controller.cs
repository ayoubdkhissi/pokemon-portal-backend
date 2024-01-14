using API.Services;
using Application.Common;
using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Common;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using System.Net;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Controller<T, TDto, TCreateDto, TUpdateDto> : ControllerBase
    where T : BaseEntity
    where TDto : IEntityDto, new()
    where TCreateDto : class
    where TUpdateDto : class
{

    protected readonly IService<T> _service;
    protected readonly IResultHandler _resultHandler;
    protected readonly ICreateValidator<TCreateDto> _createValidator;
    protected readonly IUpdateValidator<TUpdateDto> _updateValidator;
    public Controller(IService<T> service,
        IResultHandler resultHandler,
        ICreateValidator<TCreateDto> createValidator,
        IUpdateValidator<TUpdateDto> updateValidator)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _resultHandler = resultHandler ?? throw new ArgumentNullException(nameof(resultHandler));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> Get()
    {
        var entities = await _service.GetAllAsync();
        var dtos = entities.Adapt<IEnumerable<TDto>>();
        return _resultHandler.HandleResult(OperationResult.Success(dtos));
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Get(int id)
    {
        var entity = await _service.GetByIdAsync(id);
        if (entity is null)
        {
            var result = OperationResult.Failure(
                (int)HttpStatusCode.NotFound,
                ErrorCodes.NotFound);
            return _resultHandler.HandleResult(result);
        }
        var dto = entity.Adapt<TDto>();
        return _resultHandler.HandleResult(OperationResult.Success(dto));
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> Post([FromBody] TCreateDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var result = new ValidationOperationResult
            {
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorCode = ErrorCodes.ValidationFailed,
                Errors = validationResult.Errors.Select(error => new ValidationError
                {
                    ErrorCode = error.ErrorCode,
                    PropertyName = error.PropertyName
                })
            };

            return _resultHandler.HandleResult(result);
        }
        var entity = dto.Adapt<T>();
        var addedEntity = await _service.AddAsync(entity);
        var addedDto = addedEntity.Adapt<TDto>();
        return _resultHandler.HandleResult(OperationResult.Success(addedDto));
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public virtual async Task<IActionResult> Put(int id, [FromBody] TUpdateDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var result = new ValidationOperationResult
            {
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorCode = ErrorCodes.ValidationFailed,
                Errors = validationResult.Errors.Select(error => new ValidationError
                {
                    ErrorCode = error.ErrorCode,
                    PropertyName = error.PropertyName
                })
            };

            return _resultHandler.HandleResult(result);
        }
        var existingEntity = await _service.GetByIdAsync(id);
        if (existingEntity is null)
        {
            var result = OperationResult.Failure(
                (int)HttpStatusCode.NotFound,
                ErrorCodes.NotFound);
            return _resultHandler.HandleResult(result);
        }

        dto.Adapt(existingEntity);
        await _service.UpdateAsync(existingEntity);
        var updatedDto = existingEntity.Adapt<TDto>();
        return _resultHandler.HandleResult(OperationResult.Success(updatedDto));
    }

    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        var existingEntity = await _service.GetByIdAsync(id);
        if (existingEntity is null)
        {
            var result = OperationResult.Failure(
                (int)HttpStatusCode.NotFound,
                ErrorCodes.NotFound);
            return _resultHandler.HandleResult(result);
        }
        await _service.DeleteAsync(existingEntity);
        return _resultHandler.HandleResult(OperationResult.Success());
    }
}
