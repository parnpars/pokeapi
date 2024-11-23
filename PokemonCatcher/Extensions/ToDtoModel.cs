using PokemonCatcher.Models;
using PokemonCatcher.Models.DTOs;

namespace PokemonCatcher.Extensions;

public static class ToDtoModel
{
    public static PokemonDto ToDto(this PokemonDbModel pokemonDbModel, List<TypeRelationshipDbModel> types)
    {
        return new PokemonDto()
        {
            Id = pokemonDbModel.Id,
            Name = pokemonDbModel.Name,
            PokemonId = pokemonDbModel.PokemonId,
            CreatedAt = pokemonDbModel.CreatedAt,
            Types = types.Select(x => new TypeDto
            {
                Name = x.Type
            }).ToList()
        };
    }
    
    public static PokemonDto ToDto(this PokemonDbModel pokemonDbModel, string filteredType)
    {
        return new PokemonDto()
        {
            Id = pokemonDbModel.Id,
            Name = pokemonDbModel.Name,
            PokemonId = pokemonDbModel.PokemonId,
            CreatedAt = pokemonDbModel.CreatedAt,
            Types = new List<TypeDto>
            {
                new TypeDto()
                {
                    Name = filteredType
                }
            }
        };
    }
}