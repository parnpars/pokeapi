using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonCatcher.Infrastructure.Persistence.Context;

[Table("type_relationship")]
public class TypeRelationshipDbModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public Guid Id { get; set; } 
    public Guid PokemonPK { get; set; } 
    public string Type { get; set; }
}