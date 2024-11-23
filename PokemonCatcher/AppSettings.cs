namespace PokemonCatcher;

public class AppSettings
{
    public PokeDexApi PokeDexApi { get; set; }
}

public class PokeDexApi
{
    public string BaseUrl { get; set; }
}