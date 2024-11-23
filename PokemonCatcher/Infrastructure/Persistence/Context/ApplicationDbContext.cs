using PokemonCatcher.Models;

namespace PokemonCatcher.Infrastructure.Persistence.Context;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IConfiguration? _configuration;
    
    public DbSet<PokemonDbModel> Pokemons { get; set; }
    
    public DbSet<TypeRelationshipDbModel> PokemonTypeRelationships { get; set; }
    
    public ApplicationDbContext()
    {
    }
   
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration conf) : base(options)
    {
        _configuration = conf;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PokemonDbModel>().ToTable("pokemons");
        modelBuilder.Entity<TypeRelationshipDbModel>().ToTable("type_relationship");
    }
}