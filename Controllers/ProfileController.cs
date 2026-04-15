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
        public async Task<IActionResult> BurcarUsuarioPorId(int id)
        {
            var idExiste = await _userServices.VerificarUserId(id);
            if (idExiste == false) return BadRequest("Não foi encontrado usuário no sistema!");

            var user = await _userServices.BuscarUsuarioPorId(id);
            return Ok(user);
        }
    }
}
