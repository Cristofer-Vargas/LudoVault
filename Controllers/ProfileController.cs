using LudoVault.Services.Interfaces;
using LudoVault.Services.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BurcarUsuarioPorId(long id)
        {
            try
            {
                var user = await _userServices.BuscarUsuarioPorId(id);
                return Ok(user);

            } catch (Exception e)
            {
                return BadRequest($"Erro: {e.Message}");
            }
        }

        [HttpGet("{id}/ratings")]
        public async Task<IActionResult> BuscarUserRatings(long id)
        {
            try
            {
                var userRatings = await _userServices.BuscarUserRatings(id);
                return Ok(userRatings);

            } catch (Exception e)
            {
                return BadRequest($"Erro: {e.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario([FromBody]UserRequest user, long id)
        {
            try
            {
                var u = await _userServices.AtualizarUsuario(user, id);
                return Ok(u);

            } catch (Exception e)
            {
                return BadRequest($"Erro: {e.Message}");
            }
        }
    }
}
