using LudoVault.Controllers.Base;
using LudoVault.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
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