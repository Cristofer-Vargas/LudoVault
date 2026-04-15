using LudoVault.Model;
using LudoVault.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameServices _gameServices;

        public GameController(IGameServices gameServices)
        {
            _gameServices = gameServices;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarGames()
        {
            var games = await _gameServices.BuscarGames();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarGamePorId(long id)
        {
            try
            {
                var game = await _gameServices.BuscarGamePorId(id);
                if (game == null) return BadRequest("Jogo não encontrado!");

                return Ok(game);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> CriarGame([FromBody] GameModel game)
        {
            var g = await _gameServices.CriarGame(game);
            return Ok(g);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarGame([FromBody] GameModel game)
        {
            try
            {
                var g = await _gameServices.AtualizarGame(game);
                return Ok(g);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarGame(long id)
        {
            var isDel = await _gameServices.RemoverGame(id);
            if (isDel) return Ok("Jogo excluído com êxito!");

            return BadRequest("Jogo inexistente em nosso banco de dados!");
        }
    }
}
