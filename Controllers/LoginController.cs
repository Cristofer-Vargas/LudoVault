using LudoVault.Services.Interfaces;
using LudoVault.Services.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public LoginController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreatUser([FromBody] UserRequest user)
        {
            try
            {
                bool emailEmUso = await _userServices.VerificarEmailEmUsoAsync(user.Email);
                if (emailEmUso) return BadRequest("Esse email ja esta cadastrado em nosso sistema!");

                return Ok(await _userServices.CriarUsuarioAsync(user));

            } catch(Exception e)
            {
                return BadRequest($"Erro: {e.Message}");
            }
        }
    }
}
