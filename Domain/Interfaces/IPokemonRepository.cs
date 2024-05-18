using Domain.Common;
using Domain.Entities;

namespace Domain.Interfaces;
public interface  IPokemonRepository : IRepository<Pokemon>
{
    public Task<SearchResponse<Pokemon>> SearchAsync(SearchRequest searchRequest);
}
