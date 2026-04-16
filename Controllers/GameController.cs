using LudoVault.Services.Interfaces;
using LudoVault.Services.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
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
            try
            {
                var games = await _gameServices.BuscarGames();
                return Ok(games);

            } catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarGamePorId(long id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> CriarGame([FromBody] GameRequest game)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarGame([FromBody] GameRequest game)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarGame(long id)
        {
            throw new NotImplementedException();
        }
    }
}
