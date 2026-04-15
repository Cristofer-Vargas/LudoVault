using LudoVault.Model;
using LudoVault.Services.Interfaces;
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
        public async Task<IActionResult> CreatUser([FromBody] UserModel user)
        {
            bool emailEmUso = _userServices.VerificarEmailEmUso(user.Email);
            if (emailEmUso) return BadRequest("Esse email ja esta cadastrado em nosso sistema!");
            if (user.Name == "") return BadRequest("Nome de usuário não deve ser vazio!");
            if (user.Email == "") return BadRequest("Email de usuário não deve ser vazio!");

            var userCreated = _userServices.CriarUsuario(user);
            return Ok(user);
        }
    }
}
