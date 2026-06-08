using LudoVault.Domain.Model;

namespace LudoVault.Domain.Interfaces.Repositories
{
  public interface IPlatformRepository
  {
    public Task<PlatformModel>? BuscarPorId(int id);
  }
}
