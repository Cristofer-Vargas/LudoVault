namespace LudoVault.Services.Interfaces
{
  public interface IImageServices
  {
    public Task<string> ConverteParaWebpESalvaImagem(IFormFile imagem);
  }
}
