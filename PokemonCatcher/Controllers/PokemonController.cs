using PokemonCatcher.Exceptions;
using PokemonCatcher.Extensions;
using PokemonCatcher.Interfaces;
using PokemonCatcher.Models.Request;
using PokemonCatcher.Models.Response;

namespace PokemonCatcher.Controllers;

[Route("api/v1/pokemons")]
public sealed class PokemonController(IPokemonService pokemonService) : ControllerBase
{
    /// <summary>
    /// Endpoint used to catch a Pokémon
    /// </summary>
    /// <returns></returns>
    [HttpPost("catch-pokemon")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PokemonResponseModel>> CatchPokemonAsync([FromBody] CatchPokemonRequestModel body)
    {
        if (body.PokemonId is null && body.Name is null)
        {
            return BadRequest("Either pokemonId or name is required");
        }
        try
        { 
            var pokemon = body.PokemonId.HasValue
                ? await pokemonService.CatchPokemonByIdAsync(body.PokemonId.Value)
                : await pokemonService.CatchPokemonByNameAsync(body.Name);
            return Ok(pokemon.ToResponse());
        }
        catch (PokemonNotFoundException e)
        {
           return NotFound(e.Message);
        }
        catch (PokemonAlreadyCaughtException e)
        {
            BadRequest(e.Message);
        }
        
        return StatusCode(500);
    }
    
    /// <summary>
    /// Endpoint to retrieve the caught Pokémons
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("get-caught-pokemons")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ListPokemonResponseModel>> GetPokemonsAsync([FromQuery] string? filter) //Todo: ska det gå att filtrera på fler än en? comma separated filter?
    {
        var pokemons = await pokemonService.ListPokemonAsync(filter);
        return Ok(pokemons.ToResponse());
    }
    
    /// <summary>
    /// Endpoint used to release a caught Pokémon
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpDelete("release-pokemon/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReleasePokemonAsync([FromRoute] Guid id)
    {
        var released = await pokemonService.ReleasePokemonAsync(id);
        return released ? NoContent() : NotFound("Pokemon could not be found");
    }
}
