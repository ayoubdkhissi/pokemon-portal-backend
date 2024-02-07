using Domain.Common;

namespace Tests.InfrastructureTests.Utilities;
public class TestEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public List<TestRelatedEntity> TestRelatedEntities { get; set; } = new();
}

public class TestRelatedEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public TestEntity? TestEntity { get; set; }
}