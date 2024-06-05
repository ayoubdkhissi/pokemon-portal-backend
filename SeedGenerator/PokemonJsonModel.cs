class PokemonJsonModel
{
    public string Name { get; set; } = string.Empty;
    public int Attack { get; set; }
    public int Defense { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public List<int> TypeIds { get; set; } = [];
}