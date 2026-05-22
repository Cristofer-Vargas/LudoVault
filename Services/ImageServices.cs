using LudoVault.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace LudoVault.Services
{
  public class ImageServices(ISystemServices sistema, ILogger<ImageServices> logger) : IImageServices
  {
    private readonly ISystemServices _sistema = sistema;
    private readonly ILogger<ImageServices> _logger = logger;

    public async Task<string> ConverteParaWebpESalvaImagem(IFormFile imagem, string finalPath)
    {
      string caminhoGamePasta = Path.Combine(_sistema.CaminhoAssetsRoot(), "uploads", $"{finalPath}\\");
      string nomeArquivo = Guid.NewGuid().ToString() + ".webp";
      string caminhoCompleto = caminhoGamePasta + nomeArquivo;

      if (imagem == null || imagem.Length == 0)
      {
        return caminhoGamePasta + "default-image.webp";
      }

      if (!Directory.Exists(caminhoGamePasta)) Directory.CreateDirectory(caminhoGamePasta);

      using (var stream = imagem.OpenReadStream())
      {
        var img = await Image.LoadAsync(stream);
        img.Mutate(x => x.Resize(new ResizeOptions
        {
          Size = new Size(1080, 0),
          Mode = ResizeMode.Max
        }));
        await img.SaveAsWebpAsync(caminhoCompleto, new WebpEncoder { Quality = 90 }); // Converte para Webp e salva como Webp para o camingo

      }   // Carrega os Bytes de "imagem" da memória para "stream"

      _logger.LogInformation("Imagem [{ImgName}] salva em: {ImgPath}", nomeArquivo, caminhoGamePasta);
      return caminhoCompleto;
    }

    public bool ExcluirImagemAsset(string filePath)
    {
      if (!File.Exists(filePath) || filePath == null)
      {
        _logger.LogError("Erro ao excluir imagem no caminho: [{ImgPath}] -> Arquivo não existe ou caminho incorreto!", filePath);
        return false;
      }

      File.Delete(filePath);
      _logger.LogInformation("Imagem excluída de: {ImgPath}", filePath);
      return true;
    }
  }
}
