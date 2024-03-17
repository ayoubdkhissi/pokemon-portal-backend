using API.Services;
using Application.Common;
using Application.DTOs.Pokemon;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : Controller<Pokemon, PokemonDto, PokemonManipulationDto, PokemonManipulationDto>
{
    private new readonly IPokemonService _service;
    public PokemonController(
        IPokemonService service, 
        IResultHandler resultHandler, 
        ICreateValidator<PokemonManipulationDto> createValidator, 
        IUpdateValidator<PokemonManipulationDto> updateValidator,
        ILoggerAdapter<Controller<Pokemon, PokemonDto, PokemonManipulationDto, PokemonManipulationDto>> logger) 
        : base(service, resultHandler, createValidator, updateValidator, logger)
    {
        _service = service;
    }

    [HttpGet("Search")]
    public async Task<IActionResult> Search([FromQuery] SearchRequest request)
    {
        var searchResponse = await _service.SearchAsync(request);
        return _resultHandler.HandleResult(OperationResult.Success(searchResponse));
    }
}
