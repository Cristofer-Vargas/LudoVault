using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
  public interface IPublisherRepository
  {
    // Desenvolvedora
    public Task<PublisherModel>? CriarAsync(PublisherModel publisher);
    public Task<PublisherModel>? AtualizarAsync(PublisherModel publisher);
    public Task<List<PublisherModel>> BuscarTodosAsync();
    public Task<PublisherModel>? BuscarPorIdAsync(int id);
    public Task<bool> ExcluirAsync(PublisherModel publisher);
  }
}
