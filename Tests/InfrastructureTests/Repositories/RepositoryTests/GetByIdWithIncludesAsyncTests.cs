#nullable disable
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Repositories;
using Tests.InfrastructureTests.Utilities;
using Tests.TestSupport;

namespace Tests.InfrastructureTests.Repositories.RepositoryTests;

[Trait("Category", "Infra.Repositories.Repository")]
public class GetByIdWithIncludesAsyncTests : GivenWhenAsyncThenTest
{
    private TestDbContext _testDbContext;
    private IRepository<TestEntity> _sut1;
    private IRepository<TestRelatedEntity> _sut2;
    private TestEntity _resultEntity1;
    private TestRelatedEntity _resultEntity2;

    protected override void Given()
    {
        _testDbContext = InMemoryDb.CreateAndSeedInMemoryDatabase();
        _sut1 = new Repository<TestEntity>(_testDbContext);
        _sut2 = new Repository<TestRelatedEntity>(_testDbContext);
    }

    protected override async Task WhenAsync()
    {
        _resultEntity1 = await _sut1.GetByIdWithIncludesAsync(1, entity => entity.TestRelatedEntities);
        _testDbContext.ChangeTracker.Clear();
        _resultEntity2 = await _sut2.GetByIdWithIncludesAsync(1, relatedEntity => relatedEntity.TestEntity);
    }

    [Fact]
    public void Then_It_Should_Return_Expected_Entity()
    {
        _resultEntity1.Should().NotBeNull();
        _resultEntity1.Id.Should().Be(1);
        _resultEntity1.Name.Should().Be("Test 1");

        _resultEntity2.Should().NotBeNull();
        _resultEntity2.Id.Should().Be(1);
        _resultEntity2.Name.Should().Be("Related 1");
    }

    [Fact]
    public void Then_It_Should_Include_Related_Entities()
    {
        _resultEntity1.TestRelatedEntities.Should().NotBeEmpty();
    }

    [Fact]
    public void Then_It_Should_Include_Parent_Entity()
    {
        _resultEntity2.TestEntity.Should().NotBeNull();
    }
}
