using PokemonCatcher.Exceptions;
using PokemonCatcher.Extensions;
using PokemonCatcher.Interfaces;
using PokemonCatcher.Models.DTOs;
using PokemonCatcher.Models.ExternalApi;

namespace PokemonCatcher.Services;

public class PokemonService(PokeApiService pokeApiService, ApplicationDbContext context)
    : IPokemonService
{
    public async Task<PokemonDto> CatchPokemonByIdAsync(int pokemonId)
    {
        var pokeExists = await context.Pokemons.FirstOrDefaultAsync(x => x.PokemonId == pokemonId) != null;
        if (pokeExists)
            throw new PokemonAlreadyCaughtException($"Pokemon with pokemonId {pokemonId} has already been caught");
        
        var pokeApiModel = await pokeApiService.GetPokemonById(pokemonId);
        if (pokeApiModel == null)
            throw new PokemonNotFoundException($"Pokemon with pokemonId {pokemonId} could not be found");
        
        return await SavePokemonAsync(pokeApiModel);
    }

    public async Task<PokemonDto> CatchPokemonByNameAsync(string name)
    {
        var validatedName = name.Trim().ToLower();
        
        var pokeExists = await context.Pokemons.FirstOrDefaultAsync(x => x.Name == validatedName) != null;
        if (pokeExists)
            throw new PokemonAlreadyCaughtException($"Pokemon with name {validatedName} has already been caught");
        
        var pokeApiModel = await pokeApiService.GetPokemonByName(validatedName); 
        if (pokeApiModel == null)
            throw new PokemonNotFoundException($"Pokemon with name {validatedName} could not be found");
        
        return await SavePokemonAsync(pokeApiModel);
    }

    public async Task<List<PokemonDto>> ListPokemonAsync(string? filterType = null)
    {
        return filterType is null ? GetAllPokemons() : await GetFilteredPokemonsByType(filterType); 
    }
    
    public async Task<bool> ReleasePokemonAsync(Guid id)
    {
        var pokeDbModel = await context.Pokemons.FirstOrDefaultAsync(x => x.Id == id);
        if (pokeDbModel == null)
        {
            return false;
        }
        
        context.Pokemons.Remove(pokeDbModel);
        var typeRelationships = context.PokemonTypeRelationships.Where(x => x.PokemonPK == pokeDbModel.Id).ToList();
        context.RemoveRange(typeRelationships);
        await context.SaveChangesAsync();
        
        var pokemonRemoved = await context.Pokemons.FirstOrDefaultAsync(x => x.Id == id) == null;
        return pokemonRemoved;
    }

    private List<PokemonDto> GetAllPokemons()
    {
        var pokemonDbModels = context.Pokemons.ToList();
        var pokeDtos = new List<PokemonDto>();
        foreach (var pokeDbModel in pokemonDbModels)
        { 
            var typeRelationships = context.PokemonTypeRelationships.Where(x => x.PokemonPK == pokeDbModel.Id).ToList(); 
            pokeDtos.Add(pokeDbModel.ToDto(typeRelationships));
        }
        return pokeDtos;
    }

    private async Task<List<PokemonDto>> GetFilteredPokemonsByType(string filterType)
    {
        var validatedFilter = filterType.Trim().ToLower();
        
        var filteredTypePokemonIds = context.PokemonTypeRelationships
            .Where(x => x.Type == validatedFilter)
            .Select(x => x.PokemonPK)
            .ToList();
        
        var pokeDtos = new List<PokemonDto>();
        foreach (var pokeId in filteredTypePokemonIds)
        {
            var pokeDbModel = await context.Pokemons.FirstOrDefaultAsync(x => x.Id == pokeId);
            if (pokeDbModel == null)
            {
                return new List<PokemonDto>();
            }
            pokeDtos.Add(pokeDbModel.ToDto(validatedFilter));
        }
        return pokeDtos;
    }
    
    private async Task<PokemonDto> SavePokemonAsync(PokeApiModel pokeApiModel)
    {
        var pokemonDbModel = CreatePokemonDbModel(pokeApiModel);
        var pokeEntity = await context.Pokemons.AddAsync(pokemonDbModel);
        await context.SaveChangesAsync();
            
        var typeRelationships = CreateTypeRelationshipModels(pokeApiModel, pokeEntity.Entity.Id);
        await context.PokemonTypeRelationships.AddRangeAsync(typeRelationships);
        await context.SaveChangesAsync();
            
        return pokeEntity.Entity.ToDto(typeRelationships);
    }

    private PokemonDbModel CreatePokemonDbModel(PokeApiModel pokeApiModel)
    {
        return new PokemonDbModel()
        {
            Id = Guid.NewGuid(),
            Name = pokeApiModel.Name,
            PokemonId = pokeApiModel.PokemonId,
            CreatedAt = DateTime.UtcNow
        };
    }

    private List<TypeRelationshipDbModel> CreateTypeRelationshipModels(PokeApiModel pokeApiModel, Guid id)
    {
        return pokeApiModel.Types.Select(x => new TypeRelationshipDbModel()
        {
            Id = Guid.NewGuid(),
            PokemonPK = id,
            Type = x.Type.Name
        }).ToList();
    }
}