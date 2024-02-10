#nullable disable
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Repositories;
using Tests.InfrastructureTests.Utilities;
using Tests.TestSupport;

namespace Tests.InfrastructureTests.Repositories.RepositoryTests;
[Trait("Category", "Infra.Repositories.Repository")]
public class DeleteRangeTests : GivenWhenThenTest
{
    private TestDbContext _testDbContext;
    private IRepository<TestEntity> _sut;
    private List<TestEntity> _entitiesToDelete;

    protected override async void Given()
    {
        _testDbContext = InMemoryDb.CreateAndSeedInMemoryDatabase();
        _sut = new Repository<TestEntity>(_testDbContext);
        _entitiesToDelete =
        [
            await _sut.GetByIdAsync(1),
            await _sut.GetByIdAsync(2)
        ];
    }

    protected override void When()
    {
        _sut.DeleteRange(_entitiesToDelete);
    }

    [Fact]
    public void Then_It_Should_Delete_Entities_Locally()
    {
        _testDbContext.TestEntities.Local.Should().NotContain(_entitiesToDelete);
        _testDbContext.Set<TestEntity>().Local.Should().NotContain(_entitiesToDelete);
    }

    [Fact]
    public void Then_Actual_Db_Should_Contain_Entities_To_Be_Deleted()
    {
        _testDbContext.TestEntities.Should().Contain(_entitiesToDelete);
        _testDbContext.Set<TestEntity>().Should().Contain(_entitiesToDelete);
    }
}
