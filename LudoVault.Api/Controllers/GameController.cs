using LudoVault.Api.Controllers.Base;
using LudoVault.Application.DTO.Requests;
using LudoVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Api.Controllers
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
      return this.GetResponse(await _gameServices.BuscarTodosGamesAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarGamePorId(int id)
    {
      return this.GetResponse(await _gameServices.BuscarGamePorIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> CriarGame([FromBody] GameRequest game)
    {
      return this.GetResponse(await _gameServices.CriarGameAsync(game));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarGame([FromBody] GameRequest game, int id)
    {
      return this.GetResponse(await _gameServices.AtualizarGameAsync(game, id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarGame(int id)
    {
      return this.GetResponse(await _gameServices.RemoverGameAsync(id));
    }

    [HttpDelete("{gameId}/remove/profile/image")]
    public async Task<IActionResult> RemoverImagemDeCapa(int gameId)
    {
      return this.GetResponse(await _gameServices.RemoverImagemDeCapaAsync(gameId));
    }

    // Avaliações de Jogo
    [HttpGet("{id}/ratings")]
    public async Task<IActionResult> BuscarRatingsDeGame(int id)
    {
      return this.GetResponse(await _gameServices.BuscarAvaliacoesPorJogoAsync(id));
    }
  }
}
