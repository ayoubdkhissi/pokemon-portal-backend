using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;
public class PokemonRepository : Repository<Pokemon>, IPokemonRepository
{
    private readonly AppDbContext _context;
    public PokemonRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<SearchResponse<Pokemon>> SearchAsync(SearchRequest searchRequest)
    {
        IQueryable<Pokemon> query = _context.Pokemons
            .Include(p => p.Powers)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchRequest.SearchTerm))
        {
            query = query.Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{searchRequest.SearchTerm.ToLower()}%"));
            
        }

        var items = await query.Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                     .Take(searchRequest.PageSize)
                     .ToListAsync();

        var totalItems = await _context.Pokemons.CountAsync();
        return new SearchResponse<Pokemon>
        {
            Items = items,
            PageNumber = searchRequest.PageNumber,
            PageSize = searchRequest.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)searchRequest.PageSize)
        };
    }
}
