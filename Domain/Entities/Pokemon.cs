using Domain.Common;

namespace Domain.Entities;
public class Pokemon : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Attack { get; set; }
    public int Defense { get; set; }
}
