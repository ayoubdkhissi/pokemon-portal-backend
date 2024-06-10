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
        IQueryable<Pokemon> PokemonsQuery = _context.Pokemons
            .Include(p => p.Powers)
            .OrderBy(p => p.Id)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchRequest.SearchTerm))
        {
            PokemonsQuery = PokemonsQuery.Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{searchRequest.SearchTerm.ToLower()}%"));

        }

        var totalItems = await PokemonsQuery.CountAsync();

        var items = await PokemonsQuery.Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
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
        IQueryable<Pokemon> pokmeonsQuery = _context.Pokemons
            .Include(p => p.Powers)
            .AsQueryable()
            .AsNoTracking();

        IQueryable<Power> powersQuery = _context.Powers
            .Include(p => p.Pokemons)
            .AsQueryable()
            .AsNoTracking();


        var topCatchedPokemons = await pokmeonsQuery.OrderByDescending(p => p.CatchCount)
            .Take(4)
            .Select(p => new Pokemon
            {
                Name = p.Name,
                CatchCount = p.CatchCount
            }).ToListAsync();
        var catchCountCardData = new CatchCountCardData()
        {
            Labels = topCatchedPokemons.Select(p => p.Name).ToList(),
            Counts = topCatchedPokemons.Select(p => p.CatchCount).ToList()
        };


        var averagesByPower = await powersQuery
            .Select(power => new AveragesByPower
            {
                Power = new Power
                {
                    Id = power.Id,
                    Name = power.Name,
                    ImageUrl = power.ImageUrl,
                    CreatedAt = power.CreatedAt,
                    UpdatedAt = power.UpdatedAt,
                    Color = power.Color,
                    Pokemons = new List<Pokemon>()
                },
                PokemonCount = power.Pokemons.Count(),
                AvgAttack = power.Pokemons.Average(pokemon => (double)pokemon.Attack),
                AvgDefense = power.Pokemons.Average(pokemon => (double)pokemon.Defense)
            })
            .ToListAsync();


        var countCatchCountByPower = await powersQuery
            .Where(p => p.Pokemons.Any(p => p.CatchCount > 0))
            .Select(power => new CatchCountByPowerData
            {
                Power = new Power
                {
                    Id = power.Id,
                    Name = power.Name,
                    ImageUrl = power.ImageUrl,
                    CreatedAt = power.CreatedAt,
                    UpdatedAt = power.UpdatedAt,
                    Color = power.Color,
                    Pokemons = new List<Pokemon>()
                },
                Count = power.Pokemons.Sum(p => p.CatchCount)
            })
            .ToListAsync();

        return new StatisticsModel
        {
            CatchCountCardData = catchCountCardData,
            AveragesByPower = averagesByPower,
            CatchCountByPower = countCatchCountByPower
        };
    }
}
