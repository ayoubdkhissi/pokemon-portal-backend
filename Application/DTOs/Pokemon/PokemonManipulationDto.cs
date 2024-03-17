﻿namespace Application.DTOs.Pokemon;
public class PokemonManipulationDto
{
    public string Name { get; set; } = string.Empty;
    public int Attack { get; set; }
    public int Defense { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}
