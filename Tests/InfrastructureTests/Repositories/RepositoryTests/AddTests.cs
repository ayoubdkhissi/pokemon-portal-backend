#nullable disable
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Repositories;
using Tests.InfrastructureTests.Utilities;
using Tests.TestSupport;

namespace Tests.InfrastructureTests.Repositories.RepositoryTests;
[Trait("Category", "Infra.Repositories.Repository")]
public class AddTests : GivenWhenThenTest
{
    private TestDbContext _testDbContext;
    private IRepository<TestEntity> _sut;
    private TestEntity _addedEntity;

    protected override void Given()
    {
        _addedEntity = new TestEntity { Name = "Test 4" };
        _testDbContext = InMemoryDb.CreateAndSeedInMemoryDatabase();
        _sut = new Repository<TestEntity>(_testDbContext);
    }

    protected override void When()
    {
        _addedEntity = _sut.Add(_addedEntity);
    }

    [Fact]
    public void Then_It_Should_Add_Entity_Locally()
    {
        _testDbContext.TestEntities.Local.Should().Contain(_addedEntity);
        _testDbContext.Set<TestEntity>().Local.Should().Contain(_addedEntity);
    }

    [Fact]
    public void Then_Actual_Db_Should_Not_Contain_Added_Entity()
    {
        _testDbContext.TestEntities.Should().NotContain(_addedEntity);
        _testDbContext.Set<TestEntity>().Should().NotContain(_addedEntity);
    }
}
