using PokemonCatcher.Models.ExternalApi;

namespace PokemonCatcher.Extensions;

public static class ToDatabaseModel
{
    public static PokemonDbModel ToPokemonDbModel(this PokeApiModel pokeApiModel)
    {
        return new PokemonDbModel()
        {
            Id = Guid.NewGuid(),
            Name = pokeApiModel.Name,
            PokemonId = pokeApiModel.PokemonId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static List<TypeRelationshipDbModel> ToTypeRelationshipDbModel(this PokeApiModel pokeApiModel, Guid id)
    {
        return pokeApiModel.Types.Select(x => new TypeRelationshipDbModel()
        {
            Id = Guid.NewGuid(),
            PokemonPK = id,
            Type = x.Type.Name
        }).ToList();
    }
}