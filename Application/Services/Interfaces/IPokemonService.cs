using Application.Common;
using Application.DTOs.Pokemon;
using Domain.Common;
using Domain.Entities;
using Domain.Models;

namespace Application.Services.Interfaces;
public interface IPokemonService : IService<Pokemon>
{
    Task<SearchResponse<PokemonDto>> SearchAsync(SearchRequest searchRequest);
    Task<StatisticsModel> GetStatisticsAsync();
    Task<OperationResult> CatchPokemonAsync(int id);
    Task<OperationResult> ReleasePokemonAsync(int id);
    Task<OperationResult> ReleaseMultiplePokemonAsync(List<int> ids);
}
