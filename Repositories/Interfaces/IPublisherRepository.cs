using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
  public interface IPublisherRepository
  {
    // Desenvolvedora
    public Task<PublisherModel>? CriarPublisherAsync(PublisherModel publisher);
    public Task<PublisherModel>? AtualizarPublisherAsync(PublisherModel publisher);
    public Task<List<PublisherModel>> BuscarTodasPublishersAsync();
    public Task<PublisherModel>? BuscarPublisherPorIdAsync(int id);
    public Task<bool> ExcluirPublisherAsync(int id);
  }
}
