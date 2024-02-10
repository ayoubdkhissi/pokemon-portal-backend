#nullable disable
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Repositories;
using Tests.InfrastructureTests.Utilities;
using Tests.TestSupport;

namespace Tests.InfrastructureTests.Repositories.RepositoryTests;
[Trait("Category", "Infra.Repositories.Repository")]
public class UpdateTests : GivenWhenThenTest
{
    private TestDbContext _testDbContext;
    private IRepository<TestEntity> _sut;
    private TestEntity _updatedEntity;

    protected override void Given()
    {
        _testDbContext = InMemoryDb.CreateAndSeedInMemoryDatabase();
        _sut = new Repository<TestEntity>(_testDbContext);
        _updatedEntity = new TestEntity { Id = 1, Name = "Test 1 Updated" };
    }

    protected override void When()
    {
        _sut.Update(_updatedEntity);
    }

    [Fact]
    public void Then_It_Should_Update_Entity_Locally()
    {
        _testDbContext.TestEntities.Local.Should().Contain(_updatedEntity);
        _testDbContext.Set<TestEntity>().Local.Should().Contain(_updatedEntity);
        _testDbContext.TestEntities.Local.First().Name.Should().Be("Test 1 Updated");
    }

    [Fact]
    public void Then_Actual_Db_Should_Not_Contain_Updated_Entity()
    {
        _testDbContext.ChangeTracker.Clear();
        var entityInDb = _testDbContext.TestEntities.Find(1);
        entityInDb.Name.Should().Be("Test 1");
    }
}
