using LudoVault.DTO.Requests;
using LudoVault.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
  [ApiController]
  [Route("[Controller]")]
  public class GameController(IGameServices gameServices) : ControllerBase
  {
    private readonly IGameServices _gameServices = gameServices;

    // Jogo
    [HttpGet]
    public async Task<IActionResult> BuscarGames()
    {
      try
      {
        var games = await _gameServices.BuscarTodosGamesAsync();
        return Ok(games);

      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarGamePorId(int id)
    {
      try
      {
        var game = await _gameServices.BuscarGamePorIdAsync(id);
        return Ok(game);

      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }

    [HttpPost]
    public async Task<IActionResult> CriarGame([FromBody] GameRequest game)
    {
      try
      {
        var newGame = await _gameServices.CriarGameAsync(game);
        return Ok(newGame);

      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarGame([FromBody] GameRequest game, int id)
    {
      try
      {
        var gameUp = await _gameServices.AtualizarGameAsync(game, id);
        return Ok(gameUp);

      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarGame(int id)
    {
      try
      {
        await _gameServices.RemoverGameAsync(id);
        return Ok("Excluido com sucesso!");

      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    // Avaliações de Jogo
    [HttpGet("{id}/ratings")]
    public async Task<IActionResult> BuscarRatingsDeGame(int id)
    {
      try
      {
        var gameRatings = await _gameServices.BuscarAvaliacoesPorJogoAsync(id);
        return Ok(gameRatings);

      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }
  }
}
