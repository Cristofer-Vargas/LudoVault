using LudoVault.Services.Responses;

namespace LudoVault.Services.Interfaces
{
    public interface IPublisherServices
    {
        public Task<PublisherResponse> BuscarPublishers();
    }
}
