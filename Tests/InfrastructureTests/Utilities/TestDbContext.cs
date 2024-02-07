using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Tests.InfrastructureTests.Utilities;
public class TestDbContext : AppDbContext
{

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestEntity> TestEntities { get; set; }
}
