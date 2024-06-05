using Application.DTOs.Pokemon;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Mapster;
using System.Text.Json;

namespace Application.Services;
public class PokemonService : Service<Pokemon>, IPokemonService
{
    private readonly IPokemonRepository _repository;
    private readonly ICacheService _cacheService;
    private const int CACHE_EXPIRATION = 3600;

    public PokemonService(IUnitOfWork unitOfWork, ICacheService cacheService) : base(unitOfWork)
    {
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
}
