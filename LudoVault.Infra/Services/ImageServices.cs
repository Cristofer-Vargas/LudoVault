using LudoVault.Application.Interfaces.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace LudoVault.Infra.Services
{
  public class ImageServices(ILogger<ImageServices> logger, IWebHostEnvironment webHost, IConfiguration config) : IImageServices
  {
    private readonly ILogger<ImageServices> _logger = logger;
    private readonly IWebHostEnvironment _webHost = webHost;
    private readonly IConfiguration _config = config;

    public async Task<string> ConverteParaWebpESalvaImagem(IFormFile imagem, string finalPath)
    {
      if (imagem == null || imagem.Length == 0)
      {
        if (finalPath.Equals("users", StringComparison.OrdinalIgnoreCase))
        {
            return _config["ImageProvider:DefaultImages:UserAvatar"] ?? "/uploads/users/default-image.webp";
        }
        return _config["ImageProvider:DefaultImages:GameImage"] ?? "/uploads/games/default-image.webp";
      }

      // Constrói o caminho físico absoluto para salvar no disco
      string caminhoGamePasta = Path.Combine(_webHost.WebRootPath, "uploads", finalPath);
      string nomeArquivo = Guid.NewGuid().ToString() + ".webp";
      string caminhoCompleto = Path.Combine(caminhoGamePasta, nomeArquivo);

      if (!Directory.Exists(caminhoGamePasta)) Directory.CreateDirectory(caminhoGamePasta);

      using (var stream = imagem.OpenReadStream())
      {
        var img = await Image.LoadAsync(stream);
        img.Mutate(x => x.Resize(new ResizeOptions
        {
          Size = new Size(1080, 0),
          Mode = ResizeMode.Max
        }));
        await img.SaveAsWebpAsync(caminhoCompleto, new WebpEncoder { Quality = 90 });
      }

      _logger.LogInformation("Imagem [{ImgName}] salva em: {ImgPath}", nomeArquivo, caminhoGamePasta);
      string caminhoRelativo = $"/uploads/{finalPath}/{nomeArquivo}";
      return caminhoRelativo;
    }

    public bool ExcluirImagemAsset(string filePath)
    {
      if (string.IsNullOrEmpty(filePath)) return false;

      string relativePath = filePath.TrimStart('/', '\\');
      string fullPath = Path.Combine(_webHost.WebRootPath, relativePath);

      if (!File.Exists(fullPath))
      {
        _logger.LogError("Erro ao excluir imagem no caminho: [{ImgPath}] -> Arquivo não existe ou caminho incorreto!", fullPath);
        return false;
      }

      File.Delete(fullPath);
      _logger.LogInformation("Imagem excluída de: {ImgPath}", fullPath);
      return true;
    }
  }
}
