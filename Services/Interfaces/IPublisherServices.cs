using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Interfaces
{
    public interface IPublisherServices
    {
        // Desenvolvedora
        public Task<PublisherResponse> CriarPublisherAsync(PublisherRequest publisher);
        public Task<PublisherResponse> AtualizarPublisherAsync(PublisherRequest publisher, int id);
        public Task<List<PublisherResponse>> BuscarTodasPublishersAsync();
        public Task<PublisherResponse> BuscarPublisherPorIdAsync(int id);
        public Task<bool> ExcluirPublisherAsync(int id);
    }
}
