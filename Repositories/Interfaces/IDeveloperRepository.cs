using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
  public interface IDeveloperRepository
  {
    // Desenvolvedora
    public Task<DeveloperModel>? CriarAsync(DeveloperModel developer);
    public Task<DeveloperModel>? AtualizarAsync(DeveloperModel developer);
    public Task<List<DeveloperModel>> BuscarTodosAsync();
    public Task<DeveloperModel>? BuscarPorIdAsync(int id);
    public Task<bool> ExcluirAsync(DeveloperModel developer);
  }
}
