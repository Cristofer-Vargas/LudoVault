using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Responses;

namespace LudoVault.Services
{
    public class PublisherServices : IPublisherServices
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherServices(IPublisherRepository publisherReposiroty)
        {
            _publisherRepository = publisherReposiroty;
        }

        public async Task<PublisherResponse> BuscarPublishers()
        {
            throw new NotImplementedException();
        }
    }
}
