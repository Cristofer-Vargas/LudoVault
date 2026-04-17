using LudoVault.Services.Interfaces;
using LudoVault.Services.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public ProfileController(IUserServices userServices)
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
