using LudoVault.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

namespace LudoVault.Services
{
  public class ImageServices(ISystemServices sistema) : IImageServices
  {
    private readonly ISystemServices _sistema = sistema;

    /// <summary>
    /// Converte em Webp e salva arquivo de imagem em caminho root do sistemas
    /// </summary>
    /// <param name="imagem"></param>
    /// <returns>Retorna string do caminho com nome da imagem salva</returns>
    public async Task<string> ConverteParaWebpESalvaImagem(IFormFile imagem)
    {
      string caminhoGamePasta = Path.Combine(_sistema.CaminhoAssetsRoot(), "uploads", "games\\");
      string caminhoCompleto = caminhoGamePasta + Guid.NewGuid().ToString() + ".webp";

      if (imagem == null || imagem.Length == 0)
        return caminhoGamePasta + "default-image.webp";

      if (!Directory.Exists(caminhoGamePasta)) Directory.CreateDirectory(caminhoGamePasta);
      

      using (var stream = imagem.OpenReadStream())
      {
        var img = await Image.LoadAsync(stream);
        await img.SaveAsWebpAsync(caminhoCompleto, new WebpEncoder { Quality = 90 }); // Converte para Webp e salva como Webp para o camingo

      }   // Carrega os Bytes de "imagem" da memória para "stream"

      return caminhoCompleto;

    }
  }
}
