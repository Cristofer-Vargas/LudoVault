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
        public IActionResult BuscarGames()
        {
            return Ok(_gameServices.BuscarGames());
        }

        [HttpGet("{id}")]
        public IActionResult BuscarGamePorId(long id)
        {
            try
            {
                var game = _gameServices.BuscarGamePorId(id);
                if (game == null) return BadRequest("Jogo não encontrado!");

                return Ok(game);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        public IActionResult CriarGame([FromBody] GameModel game)
        {
            return Ok(_gameServices.CriarGame(game));
        }

        [HttpPut]
        public IActionResult AtualizarGame([FromBody] GameModel game)
        {
            try
            {
                var g = _gameServices.AtualizarGame(game);
                return Ok(g);
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarGame(long id)
        {
            var isDel = _gameServices.RemoverGame(id);
            if (isDel) return Ok("Jogo excluído com êxito!");

            return BadRequest("Jogo inexistente em nosso banco de dados!");
        }
    }
}
