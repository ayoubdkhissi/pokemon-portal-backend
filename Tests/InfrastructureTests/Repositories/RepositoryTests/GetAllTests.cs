#nullable disable
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Tests.InfrastructureTests.Utilities;
using Tests.TestSupport;

namespace Tests.InfrastructureTests.Repositories.RepositoryTests;
[Trait("Category", "Infra.Repositories.Repository")]
public class GetAllAsyncTests : GivenWhenAsyncThenTest
{
    private TestDbContext _testDbContext;
    private IRepository<TestEntity> _sut;
    private List<TestEntity> _expectedEntities;

    protected override void Given()
    {
        var dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique database name for each test
            .Options;

        _testDbContext = new TestDbContext(dbContextOptions);

        // Add some test data to the in-memory database
        var entities = new List<TestEntity>
        {
            new TestEntity { Id = 1, Name = "Test 1" },
            new TestEntity { Id = 2, Name = "Test 2" },
            new TestEntity { Id = 3, Name = "Test 3" }
        };
        _testDbContext.AddRange(entities);
        _testDbContext.SaveChanges();
        
        _sut = new Repository<TestEntity>(_testDbContext);
    }

    protected override async Task WhenAsync()
    {
        _expectedEntities = (await _sut.GetAllAsync()).ToList();
    }

    [Fact]
    public void Then_It_Should_Return_All_Entities()
    {
        _expectedEntities.Should().NotBeNull();
        _expectedEntities.Should().HaveCount(3);

        // Assert specific properties or conditions based on your test data
        _expectedEntities.Should().Contain(entity => entity.Id == 1 && entity.Name == "Test 1");
        _expectedEntities.Should().Contain(entity => entity.Id == 2 && entity.Name == "Test 2");
        _expectedEntities.Should().Contain(entity => entity.Id == 3 && entity.Name == "Test 3");
    }

    protected override void Cleanup()
    {
        _testDbContext.Dispose();
        base.Cleanup();
    }
}
