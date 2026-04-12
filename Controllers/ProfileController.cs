using LudoVault.Services.Interfaces;
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
        public IActionResult BurcarUsuarioPorId(int id)
        {
            var idExiste = _userServices.VerificarUserId(id);
            if (idExiste == false) return BadRequest("Não foi encontrado usuário no sistema!");

            var user = _userServices.BuscarUsuarioPorId(id);
            return Ok(user);
        }
    }
}
