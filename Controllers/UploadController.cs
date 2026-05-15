using LudoVault.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace LudoVault.Controllers
{
  [ApiController]
  [Route("[Controller]")]
  public class UploadController(IGameServices gameServices, IUserServices userServices) : Controller
  {
    private readonly IGameServices _gameServices = gameServices;
    private readonly IUserServices _userServices = userServices;

    [HttpPost("game/{gameId}/profile/image")]
    public async Task<IActionResult> AdicionarImagemAoGame([FromForm] List<IFormFile> image, [FromRoute] int gameId)
    {
      if (image == null || image.Count == 0) return BadRequest("Nenhum arquivo enviado!");
      if (image.Count > 1) return BadRequest("É aceito apenas um arquivo!");
      
      var file = image.FirstOrDefault();
      var type = file.ContentType.Split("/").FirstOrDefault();

      if (type == "image")
      {
        return Ok(await _gameServices.AdicionarImagemDeCapaAsync(file, gameId));
      }

      return BadRequest("Não é possível salvar arquivo diferente de imagem!");
    }

    [HttpPost("user/{userId}/profile/image")]
    public async Task<IActionResult> AdicionarImagemDePerfilUsuario([FromForm] List<IFormFile> image, [FromRoute] int userId)
    {
      if (image == null || image.Count == 0) return BadRequest("Nenhum arquivo enviado!");
      if (image.Count > 1) return BadRequest("É aceito apenas um arquivo!");

      var file = image.FirstOrDefault();
      var type = file.ContentType.Split("/").FirstOrDefault();

      if (type == "image")
      {
        return Ok(await _userServices.AdicionarImagemDePerfilAsync(file, userId));
      }

      return BadRequest("Não é possível salvar arquivo diferente de imagem!");
    }
  }
}
