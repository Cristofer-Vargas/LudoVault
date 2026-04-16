using LudoVault.Services.Interfaces;
using LudoVault.Services.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IUserServices _userServices;

        public LoginController(ISecurityService securityService, IUserServices userServices)
        {
            _securityService = securityService;
            _userServices = userServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreatUser([FromBody] UserRequest user)
        {
            bool emailEmUso = await _userServices.VerificarEmailEmUso(user.Email);
            if (emailEmUso) return BadRequest("Esse email ja esta cadastrado em nosso sistema!");
            if (user.Name == "") return BadRequest("Nome de usuário não deve ser vazio!");
            if (user.Email == "") return BadRequest("Email de usuário não deve ser vazio!");

            return Ok(await _userServices.CriarUsuario(user));
        }
    }
}
