using API.Services;
using Application.DTOs.Pokemon;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PokemonController : Controller<Pokemon, PokemonDto, PokemonManipulationDto, PokemonManipulationDto>
{
    public PokemonController(
        IService<Pokemon> service, 
        IResultHandler resultHandler, 
        ICreateValidator<PokemonManipulationDto> createValidator, 
        IUpdateValidator<PokemonManipulationDto> updateValidator,
        ILoggerAdapter<Controller<Pokemon, PokemonDto, PokemonManipulationDto, PokemonManipulationDto>> logger) 
        : base(service, resultHandler, createValidator, updateValidator, logger)
    {
    }
}
