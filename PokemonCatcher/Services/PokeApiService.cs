using System.Text.Json;
using PokemonCatcher.Models.ExternalApi;

namespace PokemonCatcher.Services;

public class PokeApiService(IConfiguration settings, HttpClient httpClient)
{
    private readonly string baseUrl = settings.GetValue<string>("PokeDexApi:BaseUrl") ?? "";

    public async Task<PokeApiModel?> GetPokemonById(int pokemonId)
    {
       var response = await httpClient.GetAsync($"{baseUrl}pokemon/{pokemonId}");

       var pokemon = await DeserializeResponseToPokemonApiModel(response);
       return pokemon;
    }
    
    public async Task<PokeApiModel?> GetPokemonByName(string name)
    {
        var response = await httpClient.GetAsync($"{baseUrl}pokemon/{name}");

        var pokemon = await DeserializeResponseToPokemonApiModel(response);
        return pokemon;
    }

    private async Task<PokeApiModel?> DeserializeResponseToPokemonApiModel(HttpResponseMessage response)
    {
        var jsonBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PokeApiModel>(jsonBody);
    }
}