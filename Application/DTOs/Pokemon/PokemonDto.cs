using Application.DTOs.Power;

namespace Application.DTOs.Pokemon;
public class PokemonDto : PokemonManipulationDto, IEntityDto
{
    public int Id { get; set; }
    public IEnumerable<PowerDto> Powers { get; set; } = [];
}
