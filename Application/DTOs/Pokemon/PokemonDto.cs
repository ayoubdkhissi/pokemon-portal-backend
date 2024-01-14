namespace Application.DTOs.Pokemon;
public class PokemonDto : PokemonManipulationDto, IEntityDto
{
    public int Id { get; set; }
}
