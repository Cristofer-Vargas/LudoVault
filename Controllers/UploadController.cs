using LudoVault.DTO.Responses;
using LudoVault.Services.Interfaces;
using LudoVault.Validations.Base;
using Microsoft.AspNetCore.Mvc;

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
      if (image == null || image.Count == 0)
      {
        var response = new Response<GameResponse>();
        response.Report.Add(Report.Create("Nenhum arquivo enviado!", 400));
        return Ok(response);
      }
      if (image.Count > 1)
      {
        var response = new Response<GameResponse>();
        response.Report.Add(Report.Create("É aceito apenas um arquivo!", 400));
        return Ok(response);
      }

      var file = image.FirstOrDefault();
      var type = file.ContentType.Split("/");

      if (type.LastOrDefault() == "avif")
      {
        var response = new Response<GameResponse>();
        response.Report.Add(Report.Create("O formato de imagem AVIF não é aceito!", 400));
        return Ok(response);
      }

      if (type.FirstOrDefault() == "image")
      {
        return Ok(await _gameServices.AdicionarImagemDeCapaAsync(file, gameId));
      }

      var errorResponse = new Response<GameResponse>();
      errorResponse.Report.Add(Report.Create("Não é possível salvar arquivo diferente de imagem!", 400));
      return Ok(errorResponse);
    }

    [HttpPost("user/{userId}/profile/image")]
    public async Task<IActionResult> AdicionarImagemDePerfilUsuario([FromForm] List<IFormFile> image, [FromRoute] int userId)
    {
      if (image == null || image.Count == 0)
      {
        var response = new Response<UserResponse>();
        response.Report.Add(Report.Create("Nenhum arquivo enviado!", 400));
        return Ok(response);
      }
      if (image.Count > 1)
      {
        var response = new Response<UserResponse>();
        response.Report.Add(Report.Create("É aceito apenas um arquivo!", 400));
        return Ok(response);
      }

      var file = image.FirstOrDefault();
      var extension = Path.GetExtension(file!.FileName).ToLowerInvariant();

      if (file.ContentType.Equals("image/avif", StringComparison.OrdinalIgnoreCase) || extension == ".avif")
      {
        var response = new Response<UserResponse>();
        response.Report.Add(Report.Create("O formato de imagem AVIF não é aceito!", 400));
        return Ok(response);
      }

      var type = file.ContentType.Split("/").FirstOrDefault();

      if (type == "image")
      {
        return Ok(await _userServices.AdicionarImagemDePerfilAsync(file, userId));
      }

      var errorResponse = new Response<UserResponse>();
      errorResponse.Report.Add(Report.Create("Não é possível salvar arquivo diferente de imagem!", 400));
      return Ok(errorResponse);
    }
  }
}
