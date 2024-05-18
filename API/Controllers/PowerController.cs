using API.Services;
using Application.DTOs.Power;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PowerController : Controller<Power, PowerDto, PowerManipulationDto, PowerManipulationDto>
{
    public PowerController(
        IService<Power> service,
        IResultHandler resultHandler,
        ICreateValidator<PowerManipulationDto> createValidator,
        IUpdateValidator<PowerManipulationDto> updateValidator,
        ILoggerAdapter<Controller<Power, PowerDto, PowerManipulationDto, PowerManipulationDto>> logger)
        : base(service, resultHandler, createValidator, updateValidator, logger)
    {
    }
}
