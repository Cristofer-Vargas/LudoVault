using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
    public interface IPublisherRepository
    {
        public Task<PublisherModel> Criar(PublisherModel publisher);
        public Task<PublisherModel> Atualizar(PublisherModel publisher);
        public Task<List<PublisherModel>> BuscarTodos();
        public Task<PublisherModel> BuscarPorId(long id);
        public Task<bool> Excluir(long id);
    }
}
