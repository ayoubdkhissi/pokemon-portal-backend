using Domain.Common;

namespace Domain.Entities;
public class Pokemon : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Attack { get; set; }
    public int Defense { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public IEnumerable<Power> Powers { get; set; } = new List<Power>();
    public double CatchCount { get; set; }
}
