using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence;
public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    public AppDbContext() : base()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //var pendingMigrations = Database.GetPendingMigrations();
        //if (pendingMigrations.Any())
        //{
        //    Database.Migrate();
        //}
    }


    public DbSet<Pokemon> Pokemons { get; set; }
    public DbSet<Power> Powers { get; set; }
}