using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
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
            .OrderBy(p => p.Id)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchRequest.SearchTerm))
        {
            query = query.Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{searchRequest.SearchTerm.ToLower()}%"));

        }

        var totalItems = await query.CountAsync();

        var items = await query.Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                     .Take(searchRequest.PageSize)
                     .ToListAsync();

        return new SearchResponse<Pokemon>
        {
            Items = items,
            PageNumber = searchRequest.PageNumber,
            PageSize = searchRequest.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)searchRequest.PageSize)
        };
    }

    public async Task<StatisticsModel> GetStatisticsAsync()
    {
        IQueryable<Pokemon> query = _context.Pokemons
            .Include(p => p.Powers)
            .AsNoTracking();

        var topCatchedPokemons = await query.OrderByDescending(p => p.CatchCount)
            .Take(4)
            .Select(p => new Pokemon
            {
                Name = p.Name,
                CatchCount = p.CatchCount
            }).ToListAsync();

        return new StatisticsModel
        {
            CatchCountCardData = new()
            {
                Labels = topCatchedPokemons.Select(p => p.Name).ToList(),
                Counts = topCatchedPokemons.Select(p => p.CatchCount).ToList()
            }
        };
    }
}
