namespace PokemonCatcher.Models.DTOs;

public class PokemonDto
{
    public Guid Id { get; set; }
    public int PokemonId { get; set; }
    public string Name { get; set; }
    public List<TypeDto> Types { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}