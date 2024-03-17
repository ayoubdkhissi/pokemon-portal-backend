using Application.DTOs.Pokemon;
using Domain.Common;
using Domain.Entities;

namespace Application.Services.Interfaces;
public interface IPokemonService : IService<Pokemon>
{
    Task<SearchResponse<PokemonDto>> SearchAsync(SearchRequest searchRequest);
}
