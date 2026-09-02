using LudoVault.Controllers.Base;
using LudoVault.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Controllers
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