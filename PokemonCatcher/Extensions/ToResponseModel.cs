using PokemonCatcher.Models.DTOs;
using PokemonCatcher.Models.Response;

namespace PokemonCatcher.Extensions;

public static class ToResponseModel
{
    public static PokemonResponseModel ToResponse(this PokemonDto pokemonDto)
    {
        return new PokemonResponseModel()
        {
            Id = pokemonDto.Id,
            Name = pokemonDto.Name,
            PokemonId = pokemonDto.PokemonId,
            Types = pokemonDto.Types,
            CreatedAt = pokemonDto.CreatedAt
        };
    }
    
    public static ListPokemonResponseModel ToResponse(this List<PokemonDto> pokemonDtos)
    {
        return new ListPokemonResponseModel
        {
            Items = pokemonDtos.Select(p => new PokemonResponseModel()
            {
                Id = p.Id,
                Name = p.Name,
                PokemonId = p.PokemonId,
                Types = p.Types,
                CreatedAt = p.CreatedAt
            }).ToList()
        };
    }
}