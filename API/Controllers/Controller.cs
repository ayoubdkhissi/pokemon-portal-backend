using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Controller<T, TDto, TCreateDto, TUpdateDto> : ControllerBase
    where T : BaseEntity
    where TDto : IEntityDto, new()
    where TCreateDto : class
    where TUpdateDto : class
{

    private readonly IService<T> _service;

    public Controller(IService<T> service)
    {
        _service = service;
    }
}
