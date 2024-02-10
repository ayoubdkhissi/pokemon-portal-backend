#nullable disable
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Repositories;
using Tests.InfrastructureTests.Utilities;
using Tests.TestSupport;

namespace Tests.InfrastructureTests.Repositories.RepositoryTests;
[Trait("Category", "Infra.Repositories.Repository")]
public class DeleteTests : GivenWhenThenTest
{
    private TestDbContext _testDbContext;
    private IRepository<TestEntity> _sut;
    private TestEntity _entityToDelete;

    protected override async void Given()
    {
        _testDbContext = InMemoryDb.CreateAndSeedInMemoryDatabase();
        _sut = new Repository<TestEntity>(_testDbContext);
        _entityToDelete = await _sut.GetByIdAsync(1);
    }

    protected override void When()
    {
        _sut.Delete(_entityToDelete);
    }

    [Fact]
    public void Then_It_Should_Delete_Entity_Locally()
    {
        _testDbContext.TestEntities.Local.Should().NotContain(_entityToDelete);
        _testDbContext.Set<TestEntity>().Local.Should().NotContain(_entityToDelete);
    }

    [Fact]
    public void Then_Actual_Db_Should_Contain_Entity_To_Be_Deleted()
    {
        _testDbContext.TestEntities.Should().Contain(_entityToDelete);
        _testDbContext.Set<TestEntity>().Should().Contain(_entityToDelete);
    }
}
