namespace PokemonCatcher.Exceptions;

public class PokemonAlreadyCaughtException : Exception
{
    public PokemonAlreadyCaughtException(string message)
        : base(message)
    {
    }
}