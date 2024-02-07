using Microsoft.EntityFrameworkCore;

namespace Tests.InfrastructureTests.Utilities;
public static class InMemoryDb
{

    public static TestDbContext CreateAndSeedInMemoryDatabase()
    {
        var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique database name for each test
        .Options;

        var testDbContext = new TestDbContext(dbContextOptions);

        // Add some test data to the in-memory database
        var entities = new List<TestEntity>
        {
            new TestEntity { Id = 1, Name = "Test 1", TestRelatedEntities = new () { new () { Id = 1, Name = "Related 1" } } },
            new TestEntity { Id = 2, Name = "Test 2", TestRelatedEntities = new () { new () { Id = 2, Name = "Related 2" } } },
            new TestEntity { Id = 3, Name = "Test 3", TestRelatedEntities = new () { new () { Id = 3, Name = "Related 3" } } }
        };
        
        testDbContext.AddRange(entities);
        testDbContext.SaveChanges();
        testDbContext.ChangeTracker.Clear();


        return testDbContext;
    }
}
