using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
  public interface IPlatformRepository
  {
    public Task<List<PlatformModel>> BuscarTodos();
    public Task<PlatformModel>? BuscarPorId(int id);
  }
}
