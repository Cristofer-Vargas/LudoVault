using LudoVault.DTO.Requests;
using LudoVault.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
{
  [ApiController]
  [Route("[Controller]")]
  public class LoginController(IUserServices userServices) : ControllerBase
  {
    private readonly IUserServices _userServices = userServices;

    [HttpPost]
    public async Task<IActionResult> CreatUser([FromBody] UserRequest user)
    {
      return Ok(await _userServices.CriarUsuarioAsync(user));
    }
  }
}
