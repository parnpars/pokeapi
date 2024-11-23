using System.Text.Json;
using PokemonCatcher.Exceptions;
using PokemonCatcher.Models.ExternalApi;

namespace PokemonCatcher.Services;

public class PokeApiService(IConfiguration settings, HttpClient httpClient)
{
    private readonly string baseUrl = settings.GetValue<string>("PokeDexApi:BaseUrl") ?? "";

    public async Task<PokeApiModel?> GetPokemonById(int pokemonId)
    {
        try
        {
            var response = await httpClient.GetAsync($"{baseUrl}pokemon/{pokemonId}");

            var pokemon = await DeserializeResponseToPokemonApiModel(response);
            return pokemon;
        }
        catch (Exception)
        {
            throw new PokemonNotFoundException($"Pokemon with id {pokemonId} could not be found");
        }
       
    }
    
    public async Task<PokeApiModel?> GetPokemonByName(string name)
    {
        try
        {
            var response = await httpClient.GetAsync($"{baseUrl}pokemon/{name}"); 
            var pokemon = await DeserializeResponseToPokemonApiModel(response); 
            return pokemon;
        }
        catch (Exception)
        {
            throw new PokemonNotFoundException($"Pokemon with name {name} could not be found");
        }
    }

    private async Task<PokeApiModel?> DeserializeResponseToPokemonApiModel(HttpResponseMessage response)
    {
        var jsonBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<PokeApiModel>(jsonBody);
    }
}