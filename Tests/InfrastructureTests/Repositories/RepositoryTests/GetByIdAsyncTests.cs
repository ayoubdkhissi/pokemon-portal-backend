#nullable disable
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.InfrastructureTests.Utilities;
using Tests.TestSupport;

namespace Tests.InfrastructureTests.Repositories.RepositoryTests;

[Trait("Category", "Infra.Repositories.Repository")]
public class GetByIdAsyncTests : GivenWhenAsyncThenTest
{
    private TestDbContext _testDbContext;
    private IRepository<TestEntity> _sut;
    private TestEntity _expectedEntity;
    private TestEntity _expectedNullEntity;

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
        _expectedEntity = await _sut.GetByIdAsync(1);
        _expectedNullEntity = await _sut.GetByIdAsync(4);
    }

    [Fact]
    public void Then_It_Should_Return_Expected_Entity()
    {
        _expectedEntity.Should().NotBeNull();
        _expectedEntity.Id.Should().Be(1);
        _expectedEntity.Name.Should().Be("Test 1");
    }

    [Fact]
    public void Then_It_Should_Return_Null_When_Entity_Not_Found()
    {
        _expectedNullEntity.Should().BeNull();
    }
    protected override void Cleanup()
    {
        _testDbContext.Dispose();
        base.Cleanup();
    }
}
