using Domain.Common;
using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces;
public interface IPokemonRepository : IRepository<Pokemon>
{
    Task<SearchResponse<Pokemon>> SearchAsync(SearchRequest searchRequest);
    Task<StatisticsModel> GetStatisticsAsync();
}
