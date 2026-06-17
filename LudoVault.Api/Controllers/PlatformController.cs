using LudoVault.Api.Controllers.Base;
using LudoVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Api.Controllers
{
  [ApiController]
  [Route("[Controller]")]
  public class PlatformController(IPlatformServices platformServices) : ControllerBase
  {
    private readonly IPlatformServices _platformServices = platformServices;

    [HttpGet]
    public async Task<IActionResult> BuscarPlataformas()
    {
      return this.GetResponse(await _platformServices.BuscarPlataformas());
    }
  }
}