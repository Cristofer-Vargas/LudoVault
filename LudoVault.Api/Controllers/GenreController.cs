using LudoVault.Api.Controllers.Base;
using LudoVault.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Api.Controllers
{
  [ApiController]
  [Route("[Controller]")]
  public class GenreController(IGenreServices genreServices) : ControllerBase
  {
    private readonly IGenreServices _genreServices = genreServices;

    [HttpGet]
    public async Task<IActionResult> BuscarGeneros()
    {
      return this.GetResponse(await _genreServices.BuscarGeneros());
    }
  }
}