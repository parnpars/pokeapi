namespace PokemonCatcher.Models.Request;

public class CatchPokemonRequestModel
{
    public int? PokemonId { get; set; } //= default!;

    public string? Name { get; set; } //= default!;
}