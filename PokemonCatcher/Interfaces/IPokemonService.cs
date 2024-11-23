using PokemonCatcher.Models.DTOs;

namespace PokemonCatcher.Interfaces;

public interface IPokemonService
{
    public Task<PokemonDto> CatchPokemonByIdAsync(int id);
    
    public Task<PokemonDto> CatchPokemonByNameAsync(string name);

    public Task<List<PokemonDto>> ListPokemonAsync(string? filterType = null);

    public Task<bool> ReleasePokemonAsync(Guid id);
}