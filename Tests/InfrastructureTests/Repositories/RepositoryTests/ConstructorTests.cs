#nullable disable
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Moq;
using Tests.InfrastructureTests.Utilities;
using Tests.TestSupport;

namespace Tests.InfrastructureTests.Repositories.RepositoryTests;

public class ConstructorTests
{
    [Trait("Category", "Infra.Repositories.Repository")]
    public class GivenValidParameters : GivenWhenThenTest
    {
        private Mock<AppDbContext> _dbContextMock;
        private IRepository<TestEntity> _sut;

        protected override void Given()
        {
            _dbContextMock = new Mock<AppDbContext>();
        }

        protected override void When()
        {
            _sut = new Repository<TestEntity>(_dbContextMock.Object);
        }

        [Fact]
        public void Then_Constructor_Should_Create_Instance()
        {
            _sut.Should().NotBeNull();
        }
    }


    [Trait("Category", "Infra.Repositories.Repository")]
    public class GivenNullParameters : GivenWhenThenTest
    {
        private ArgumentNullException _exception = null;

        protected override void Given()
        {
        }

        protected override void When()
        {
            try
            {
                _ = new Repository<TestEntity>(null);
            }
            catch(ArgumentNullException ex)
            {
                _exception = ex;
            }
        }

        [Fact]
        public void Then_Constructor_Should_Throw_ArgumentNullException_When_Dependencies_Are_Null()
        {
            _exception.Should().NotBeNull();
        }

    }
}
