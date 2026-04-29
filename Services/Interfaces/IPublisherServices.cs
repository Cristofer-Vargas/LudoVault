using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Interfaces
{
    public interface IPublisherServices
    {
        public Task<PublisherResponse> CriarPublisher(PublisherRequest publisher);
        public Task<PublisherResponse> AtualizarPublisher(PublisherRequest publisher, int id);
        public Task<List<PublisherResponse>> BuscarPublishers();
        public Task<PublisherResponse> BuscarPublisherPorId(int id);
        public Task<bool> ExcluirPublisher(int id);
    }
}
