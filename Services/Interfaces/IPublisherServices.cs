using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Interfaces
{
    public interface IPublisherServices
    {
        public Task<PublisherResponse> CriarPublisher(PublisherRequest publisher);
        public Task<PublisherResponse> AtualizarPublisher(PublisherRequest publisher, long id);
        public Task<List<PublisherResponse>> BuscarPublishers();
        public Task<PublisherResponse> BuscarPublisherPorId(long id);
        public Task<bool> ExcluirPublisher(long id);
    }
}
