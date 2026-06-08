using LudoVault.Application.DTO.Requests;
using LudoVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Api.Controllers
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
