using LudoVault.Services.Interfaces;

namespace LudoVault.Services
{
  public class SystemServices(IWebHostEnvironment sistema) : ISystemServices
  {
    private readonly IWebHostEnvironment _sistema = sistema;

    public string CaminhoAssetsRoot()
    {
      return _sistema.WebRootPath;
    }

    public string CaminhoGameDefaultImage()
    {
      return Path.Combine(_sistema.WebRootPath, "uploads", "games", "default-image.webp");
    }

    public string CaminhoUserDefaultImage()
    {
      return Path.Combine(_sistema.WebRootPath, "uploads", "users", "default-image.webp");
    }
  }
}
