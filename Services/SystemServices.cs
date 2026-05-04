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
  }
}
