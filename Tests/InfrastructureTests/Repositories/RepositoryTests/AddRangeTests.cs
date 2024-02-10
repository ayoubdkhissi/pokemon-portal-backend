#nullable disable
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Repositories;
using Tests.InfrastructureTests.Utilities;
using Tests.TestSupport;

namespace Tests.InfrastructureTests.Repositories.RepositoryTests;
[Trait("Category", "Infra.Repositories.Repository")]
public class AddRangeTests : GivenWhenThenTest
{
    private TestDbContext _testDbContext;
    private IRepository<TestEntity> _sut;
    private List<TestEntity> _addedEntities;

    protected override void Given()
    {
        _addedEntities = [
            new TestEntity { Name = "Test 4" },
            new TestEntity { Name = "Test 5" }
        ];

        _testDbContext = InMemoryDb.CreateAndSeedInMemoryDatabase();
        _sut = new Repository<TestEntity>(_testDbContext);
    }

    protected override void When()
    {
        _sut.AddRange(_addedEntities);
    }

    [Fact]
    public void Then_It_Should_Add_Entities_Locally()
    {
        _testDbContext.TestEntities.Local.Should().Contain(_addedEntities);
        _testDbContext.Set<TestEntity>().Local.Should().Contain(_addedEntities);
    }

    [Fact]
    public void Then_Actual_Db_Should_Not_Contain_Added_Entities()
    {
        _testDbContext.TestEntities.Should().NotContain(_addedEntities);
        _testDbContext.Set<TestEntity>().Should().NotContain(_addedEntities);
    }
}
