using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Tests.InfrastructureTests.Utilities;
public class TestDbContext : AppDbContext
{

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public TestDbContext() : base()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
    }
    public DbSet<TestEntity> TestEntities { get; set; }
    public DbSet<TestRelatedEntity> TestRelatedEntities { get; set; }
}
