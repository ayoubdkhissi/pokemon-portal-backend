using Application.DTOs.Pokemon;
using Application.Services.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Mapster;

namespace Application.Services;
public class PokemonService : Service<Pokemon>, IPokemonService
{
    private readonly IPokemonRepository _repository;
    public PokemonService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _repository = (IPokemonRepository)unitOfWork.Repository<Pokemon>();
    }

    public async Task<SearchResponse<PokemonDto>> SearchAsync(SearchRequest searchRequest)
    {
        var result = await _repository.SearchAsync(searchRequest);
        return new SearchResponse<PokemonDto>
        {
            Items = result.Items.Adapt<IEnumerable<PokemonDto>>(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalItems = result.TotalItems,
            TotalPages = result.TotalPages
        };
    }
}
