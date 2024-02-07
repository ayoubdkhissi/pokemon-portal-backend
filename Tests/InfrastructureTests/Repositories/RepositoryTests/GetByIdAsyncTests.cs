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
    private TestEntity _resultEntity1;
    private TestEntity _resultEntity2;

    protected override void Given()
    {
        _testDbContext = InMemoryDb.CreateAndSeedInMemoryDatabase();
        _sut = new Repository<TestEntity>(_testDbContext);
    }

    protected override async Task WhenAsync()
    {
        _resultEntity1 = await _sut.GetByIdAsync(1); // entity with id 1 exists (see InMemoryDb.CreateAndSeedInMemoryDatabase()) 
        _resultEntity2 = await _sut.GetByIdAsync(4); // entity with id 2 does not exist (see InMemoryDb.CreateAndSeedInMemoryDatabase())
    }

    [Fact]
    public void Then_It_Should_Return_Expected_Entity()
    {
        _resultEntity1.Should().NotBeNull();
        _resultEntity1.Id.Should().Be(1);
        _resultEntity1.Name.Should().Be("Test 1");
    }

    [Fact]
    public void Then_It_Should_Return_Null_When_Entity_Not_Found()
    {
        _resultEntity2.Should().BeNull();
    }

    [Fact]
    public void Then_It_Should_Not_Include_Related_Entities()
    {
        _resultEntity1.TestRelatedEntities.Should().BeEmpty();
    }
    
    protected override void Cleanup()
    {
        _testDbContext.Dispose();
        base.Cleanup();
    }
}
