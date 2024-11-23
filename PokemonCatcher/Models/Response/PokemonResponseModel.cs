using PokemonCatcher.Models.DTOs;

namespace PokemonCatcher.Models.Response;

public class PokemonResponseModel
{
    public Guid Id { get; set; }
    public int PokemonId { get; set; }
    public string Name { get; set; }
    public List<TypeDto> Types { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}