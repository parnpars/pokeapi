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
    /// <returns>PokemonResponseModel</returns>
    [HttpPost("catch-pokemon")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PokemonResponseModel>> CatchPokemonAsync([FromBody] CatchPokemonRequestModel body)
    {
        try
        {
            if (body is null || (body.PokemonId == null && string.IsNullOrWhiteSpace(body.Name)))
                return BadRequest("pokemon name or id is required");
            
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
            return BadRequest(e.Message);
        }
    }
    
    /// <summary>
    /// Endpoint to retrieve the caught Pokémons
    /// </summary>
    /// <returns>ListPokemonResponseModel</returns>
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
    [HttpDelete("release-pokemon/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReleasePokemonAsync([FromRoute] Guid id)
    {
        var released = await pokemonService.ReleasePokemonAsync(id);
        return released ? NoContent() : NotFound("Pokemon could not be found");
    }
}
