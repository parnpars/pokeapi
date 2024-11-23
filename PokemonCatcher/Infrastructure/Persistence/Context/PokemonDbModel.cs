using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonCatcher.Infrastructure.Persistence.Context;

[Table("pokemons")]
public class PokemonDbModel
{
    [Key]

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public int PokemonId { get; set; }
    
    public string Name { get; set; }
    
    public DateTime CreatedAt { get; set; }
}