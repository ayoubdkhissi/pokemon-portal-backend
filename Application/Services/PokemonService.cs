using Application.Common;
using Application.DTOs.Pokemon;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Shared.Constants;
using System.Net;
using System.Text.Json;

namespace Application.Services;
public class PokemonService : Service<Pokemon>, IPokemonService
{
    private readonly IPokemonRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private const int CACHE_EXPIRATION = 3600;

    public PokemonService(IUnitOfWork unitOfWork, ICacheService cacheService) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = (IPokemonRepository)unitOfWork.Repository<Pokemon>();
        _cacheService = cacheService;
    }

    public async Task<SearchResponse<PokemonDto>> SearchAsync(SearchRequest searchRequest)
    {
        var cacheKey = $"pokemon_search_{searchRequest.PageNumber}_{searchRequest.PageSize}_{searchRequest.SearchTerm}";
        var cachedResult = await _cacheService.GetAsync<SearchResponse<PokemonDto>>(cacheKey);
        if (cachedResult != null)
        {
            return cachedResult;
        }


        var result = await _repository.SearchAsync(searchRequest);
        var searchResponse = new SearchResponse<PokemonDto>
        {
            Items = result.Items.Adapt<IEnumerable<PokemonDto>>(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalItems = result.TotalItems,
            TotalPages = result.TotalPages
        };
        await _cacheService.SetAsync(cacheKey, JsonSerializer.Serialize(searchResponse), CACHE_EXPIRATION);
        return searchResponse;
    }

    public override async Task<IEnumerable<Pokemon>> GetAllAsync()
    {
        var cacheKey = "pokemon_get_all";
        var cachedResult = await _cacheService.GetAsync<IEnumerable<Pokemon>>(cacheKey);
        if (cachedResult != null)
        {
            return cachedResult;
        }

        var result = await _repository.GetAllAsync();
        await _cacheService.SetAsync(cacheKey, JsonSerializer.Serialize(result), CACHE_EXPIRATION);
        return result;
    }

    public async Task<StatisticsModel> GetStatisticsAsync()
    {
        return await _repository.GetStatisticsAsync();
    }

    public async Task<OperationResult> CatchPokemonAsync(int id)
    {
        var pokemon = await _repository.GetByIdAsync(id);
        if (pokemon == null)
        {
            return OperationResult.Failure((int)HttpStatusCode.NotFound, ErrorCodes.NotFound, $"Pokemon {id} Not Found");
        }
        pokemon.CatchCount++;
        _repository.Update(pokemon);
        await _unitOfWork.SaveChangesAsync();
        return OperationResult.Success();
    }

    public async Task<OperationResult> ReleasePokemonAsync(int id)
    {
        var pokemon = await _repository.GetByIdAsync(id);
        if (pokemon == null)
        {
            return OperationResult.Failure((int)HttpStatusCode.NotFound, ErrorCodes.NotFound, $"Pokemon {id} Not Found");
        }
        if(pokemon.CatchCount <= 0)
        {
            return OperationResult.Failure((int)HttpStatusCode.BadRequest, ErrorCodes.BadRequest, $"Pokemon {id} is not caught yet");
        }
        pokemon.CatchCount--;
        _repository.Update(pokemon);
        await _unitOfWork.SaveChangesAsync();
        return OperationResult.Success();


    }

    public async Task<OperationResult> ReleaseMultiplePokemonAsync(List<int> ids)
    {
        var pokemons = await _repository.GetByConditionAsync(p => ids.Contains(p.Id));
        foreach (var pokemon in pokemons)
        {
            if (pokemon.CatchCount <= 0)
            {
                return OperationResult.Failure((int)HttpStatusCode.BadRequest, ErrorCodes.BadRequest, $"Pokemon {pokemon.Id} is not caught yet");
            }
            pokemon.CatchCount--;
            _repository.Update(pokemon);
        }
        await _unitOfWork.SaveChangesAsync();
        return OperationResult.Success(message: $"Released {pokemons.Count()} pokemons");
    }
}
