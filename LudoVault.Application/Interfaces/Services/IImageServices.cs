namespace LudoVault.Application.Interfaces.Services
{
  public interface IImageServices
  {
    public Task<string> ConverteParaWebpESalvaImagem(IFormFile imagem, string finalPath);
    public bool ExcluirImagemAsset(string filePath);
  }
}
