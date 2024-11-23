using System.Text.Json.Serialization;

namespace PokemonCatcher.Models.ExternalApi;

public class PokeApiModel
{
    [JsonPropertyName("id")]
    public int PokemonId { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("types")]
    public List<PokeTypes> Types { get; set; }
}

public class PokeTypes
{
    [JsonPropertyName("type")]
    public PokeType Type { get; set; }
}

public class PokeType
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
}