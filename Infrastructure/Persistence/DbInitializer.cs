using Microsoft.EntityFrameworkCore;

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
            }
        }   
    }
}
