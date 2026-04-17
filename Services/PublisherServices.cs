using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Requests;
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

        public async Task<PublisherResponse> CriarPublisher(PublisherRequest publisher)
        {
            var publisherModel = await _publisherRepository.Criar(PublisherMapper.ToModel(publisher));
            return PublisherMapper.ToResponse(
                publisherModel, 
                publisherModel.Games.Select(GameMapper.ToResponse).ToList());
        }

        public async Task<PublisherResponse> AtualizarPublisher(PublisherRequest publisher, long id)
        {
            var publisherModel = PublisherMapper.ToModel(publisher);
            publisherModel.Id = id;
            var updatedPublisher = await _publisherRepository.Atualizar(publisherModel);

            return PublisherMapper.ToResponse(
                updatedPublisher,
                updatedPublisher.Games.Select(GameMapper.ToResponse).ToList());
        }

        public async Task<List<PublisherResponse>> BuscarPublishers()
        {
            List<PublisherModel> pubModelList = await _publisherRepository.BuscarTodos();

            return pubModelList.Select(publisher => PublisherMapper.ToResponse(
                publisher, 
                publisher.Games.Select(GameMapper.ToResponse)
            .ToList())).ToList();
        }

        public async Task<PublisherResponse> BuscarPublisherPorId(long id)
        {
            var publisherModel = await _publisherRepository.BuscarPorId(id);
            return PublisherMapper.ToResponse(
                publisherModel, 
                publisherModel.Games.Select(GameMapper.ToResponse).ToList()
                );
        }

        public async Task<bool> ExcluirPublisher(long id)
        {
            bool publisherExcluded = await _publisherRepository.Excluir(id);
            if (publisherExcluded) return true;
            return false;
        }
    }
}
