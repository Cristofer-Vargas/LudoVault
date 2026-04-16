using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
    public interface IPublisherRepository
    {
        public Task<List<PublisherModel>> BuscarTodos();
    }
}
