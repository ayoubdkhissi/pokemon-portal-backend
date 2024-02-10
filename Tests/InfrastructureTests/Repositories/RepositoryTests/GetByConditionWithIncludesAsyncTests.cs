#nullable disable
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Repositories;
using Tests.InfrastructureTests.Utilities;
using Tests.TestSupport;

namespace Tests.InfrastructureTests.Repositories.RepositoryTests;
[Trait("Category", "Infra.Repositories.Repository")]
public class GetByConditionWithIncludesAsyncTests : GivenWhenAsyncThenTest
{
    private TestDbContext _testDbContext;
    private IRepository<TestEntity> _sut;
    private List<TestEntity> _resultEntities;

    protected override void Given()
    {
        _testDbContext = InMemoryDb.CreateAndSeedInMemoryDatabase();
        _sut = new Repository<TestEntity>(_testDbContext);
    }

    protected override async Task WhenAsync()
    {
        _resultEntities = (await _sut.GetByConditionWithIncludesAsync(entity => entity.Name == "Test 1", entity => entity.TestRelatedEntities)).ToList();
    }

    [Fact]
    public void Then_It_Should_Return_Expected_Entities()
    {
        _resultEntities.Should().NotBeNull();
        _resultEntities.Should().HaveCount(1);
        _resultEntities.Should().Contain(entity => entity.Id == 1 && entity.Name == "Test 1");
    }

    [Fact]
    public void Then_It_Should_Not_Return_Other_Entities()
    {
        _resultEntities.Should().NotContain(entity => entity.Id == 2 && entity.Name == "Test 2");
        _resultEntities.Should().NotContain(entity => entity.Id == 3 && entity.Name == "Test 3");
    }

    [Fact]
    public void Then_It_Should_Include_Related_Entities()
    {
        _resultEntities.Should().NotBeEmpty();
        _resultEntities.ForEach(entity => entity.TestRelatedEntities.Should().NotBeEmpty());
    }
}
