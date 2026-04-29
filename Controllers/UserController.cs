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
        public async Task<IActionResult> BurcarUsuarioPorId(int id)
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
        public async Task<IActionResult> BuscarUserRatings(int id)
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

        [HttpGet("{id}/lists")]
        public async Task<IActionResult> BuscarUserLists(int id)
        {
            try
            {
                var userLists = await _userServices.BuscarUserLists(id);
                return Ok(userLists);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario([FromBody]UserRequest user, int id)
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
