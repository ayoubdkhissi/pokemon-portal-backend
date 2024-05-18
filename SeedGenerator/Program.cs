using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

HttpClient client = new HttpClient();
const string API_URL = "https://pokeapi.co/api/v2/pokemon/";
const int POKEMONS_COUNT = 1025;

List<PokemonJsonModel> pokemons = [];
for (int i = 1; i <= POKEMONS_COUNT; i++)
{
    var pokemon = await GetPokemonAsync(i);
    if (pokemon != null)
    {
        pokemons.Add(pokemon);
        Console.WriteLine($"Pokemon {pokemon.Name} has been added.");
    }
}

var jsonOutput = JsonConvert.SerializeObject(pokemons, Formatting.Indented);
var basePath = Directory.GetCurrentDirectory();
var outputPath = Path.Combine(basePath, "pokemons.json");
File.WriteAllText(outputPath, jsonOutput);
Console.WriteLine($"Pokemon data has been written to {outputPath}");


async Task<PokemonJsonModel?> GetPokemonAsync(int id)
{
    try
    {
        HttpResponseMessage response = await client.GetAsync(API_URL + id);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        JObject? result = JsonConvert.DeserializeObject<JObject>(responseBody);

        var stats = result?["stats"];
        var attack = stats?.FirstOrDefault(s => (string?)s?["stat"]?["name"] == "attack")?["base_stat"];
        var defense = stats?.FirstOrDefault(s => (string?)s?["stat"]?["name"] == "defense")?["base_stat"];
        var imageUrl = result?["sprites"]?["other"]?["dream_world"]?["front_default"];
        var types = result?["types"];

        if (attack?.Type == JTokenType.Null || defense?.Type == JTokenType.Null || imageUrl?.Type == JTokenType.Null || types?.Type == JTokenType.Null)
        {
            return null;
        }

        List<int> typeIds = new List<int>();
        foreach (var type in types!)
        {
            var typeName = (string?)type?["type"]?["name"];
            switch (typeName)
            {
                case "dark":
                    typeIds.Add(1);
                    break;
                case "electric":
                    typeIds.Add(2);
                    break;
                case "fairy":
                    typeIds.Add(3);
                    break;
                case "fighting":
                    typeIds.Add(4);
                    break;
                case "ground":
                    typeIds.Add(5);
                    break;
                case "ice":
                    typeIds.Add(6);
                    break;
                case "normal":
                    typeIds.Add(7);
                    break;
                case "poison":
                    typeIds.Add(8);
                    break;
                case "psychic":
                    typeIds.Add(9);
                    break;
                case "rock":
                    typeIds.Add(10);
                    break;
                case "steel":
                    typeIds.Add(11);
                    break;
                case "water":
                    typeIds.Add(12);
                    break;
                default:
                    break;
            }
        }

        if (!typeIds.Any())
        {
            return null;
        }
        var name = (string)result!["name"]!;

        return new PokemonJsonModel
        {
            Name = name.Substring(0, 1).ToUpper() + name.Substring(1),
            Attack = (int)attack!,
            Defense = (int)defense!,
            ImageUrl = (string)imageUrl!,
            TypeIds = typeIds
        };
    }
    catch
    {
        // If there's any error (e.g., network issue, API issue), skip this Pokemon.
        return null;
    }
}
