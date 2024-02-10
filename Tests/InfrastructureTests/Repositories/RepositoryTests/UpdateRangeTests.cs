#nullable disable
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Repositories;
using Tests.InfrastructureTests.Utilities;
using Tests.TestSupport;

namespace Tests.InfrastructureTests.Repositories.RepositoryTests;
[Trait("Category", "Infra.Repositories.Repository")]
public class UpdateRangeTests : GivenWhenThenTest
{
    private TestDbContext _testDbContext;
    private IRepository<TestEntity> _sut;
    private List<TestEntity> _updatedEntities;

    protected override void Given()
    {
        _updatedEntities = [
            new TestEntity { Id = 1, Name = "Test 1 Updated" },
            new TestEntity { Id = 2, Name = "Test 2 Updated" }
        ];

        _testDbContext = InMemoryDb.CreateAndSeedInMemoryDatabase();
        _sut = new Repository<TestEntity>(_testDbContext);
    }

    protected override void When()
    {
        _sut.UpdateRange(_updatedEntities);
    }

    [Fact]
    public void Then_It_Should_Update_Entities_Locally()
    {
        _testDbContext.TestEntities.Local.Should().Contain(_updatedEntities);
        _testDbContext.Set<TestEntity>().Local.Should().Contain(_updatedEntities);
        _testDbContext.TestEntities.Local.First().Name.Should().Be("Test 1 Updated");
        _testDbContext.TestEntities.Local.Last().Name.Should().Be("Test 2 Updated");
    }

    [Fact]
    public void Then_Actual_Db_Should_Not_Update_Entities()
    {
        _testDbContext.ChangeTracker.Clear();
        var entityInDb = _testDbContext.TestEntities.Find(1);
        entityInDb.Name.Should().Be("Test 1");
        entityInDb = _testDbContext.TestEntities.Find(2);
        entityInDb.Name.Should().Be("Test 2");
    }
}
