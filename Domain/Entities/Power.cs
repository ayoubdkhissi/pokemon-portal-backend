using Domain.Common;

namespace Domain.Entities;
public class Power : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public IEnumerable<Pokemon> Pokemons { get; set; } = new List<Pokemon>();
}
