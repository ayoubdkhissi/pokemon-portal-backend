using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Persistence;
public class DbInitializer
{
    private readonly AppDbContext _context;

    public DbInitializer(AppDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        if (_context.Database.IsNpgsql())
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await _context.Database.EnsureDeletedAsync();
                await _context.Database.MigrateAsync();
                await Seed();
            }
        }
    }

    private async Task Seed()
    {
        var powersJson = "Infrastructure.Persistence.Seed.powers.json";
        var pokemonsJson = "Infrastructure.Persistence.Seed.pokemons.json";

        // read the 2 files as Embeded Resources
        using var powersStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(powersJson);
        using var pokemonsStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pokemonsJson);

        if (powersStream is null || pokemonsStream is null)
        {
            throw new Exception("Seed files data not found");
        }

        using var powersReader = new StreamReader(powersStream);
        using var pokemonsReader = new StreamReader(pokemonsStream);

        var powersJsonString = powersReader.ReadToEnd();
        var pokemonsJsonString = pokemonsReader.ReadToEnd();

        var powers = JsonSerializer.Deserialize<List<Power>>(powersJsonString);

        // insert the powers
        await _context.Powers.AddRangeAsync(powers);
        await _context.SaveChangesAsync();

        var pokemonModels = JsonSerializer.Deserialize<List<PokemonJsonModel>>(pokemonsJsonString);
        var pokemons = pokemonModels.Select(p => new Pokemon
        {
            Name = p.Name,
            Attack = p.Attack,
            Defense = p.Defense,
            ImageUrl = p.ImageUrl,
            Powers = p.TypeIds.Select(typeId => powers.Single(p => p.Id == typeId)).ToList()
        }).ToList();

        // insert the pokemons
        await _context.Pokemons.AddRangeAsync(pokemons);
        await _context.SaveChangesAsync();

    }

}
