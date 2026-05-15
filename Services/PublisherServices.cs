using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Validations.Base;

namespace LudoVault.Services
{
  public class PublisherServices(IPublisherRepository publisherReposiroty) : IPublisherServices
  {
    private readonly IPublisherRepository _publisherRepository = publisherReposiroty;

    public async Task<Response<PublisherResponse>> CriarPublisherAsync(PublisherRequest publisher)
    {
      var publisherModel = await _publisherRepository.CriarPublisherAsync(PublisherMapper.ToModel(publisher));
      var pubRes = PublisherMapper.ToResponse(
              publisherModel,
              publisherModel.Games.Select(GameMapper.ToResponse).ToList());

      return Response.Ok(pubRes);
    }
    public async Task<Response<PublisherResponse>> AtualizarPublisherAsync(PublisherRequest publisher, int id)
    {
      var publisherModel = PublisherMapper.ToModel(publisher);
      publisherModel.Id = id;
      var updatedPublisher = await _publisherRepository.AtualizarPublisherAsync(publisherModel);

      var pubRes = PublisherMapper.ToResponse(
              updatedPublisher,
              updatedPublisher.Games.Select(GameMapper.ToResponse).ToList());

      return Response.Ok(pubRes);
    }
    public async Task<Response<List<PublisherResponse>>> BuscarTodasPublishersAsync()
    {
      List<PublisherModel> pubModelList = await _publisherRepository.BuscarTodasPublishersAsync();

      var pubRes = pubModelList.Select(publisher => PublisherMapper.ToResponse(
              publisher,
              publisher.Games.Select(GameMapper.ToResponse)
      .ToList())).ToList();

      return Response.Ok(pubRes);
    }
    public async Task<Response<PublisherResponse>> BuscarPublisherPorIdAsync(int id)
    {
      var publisherModel = await _publisherRepository.BuscarPublisherPorIdAsync(id);
      var pubRes = PublisherMapper.ToResponse(
              publisherModel,
              publisherModel.Games.Select(GameMapper.ToResponse).ToList()
              );

      return Response.Ok(pubRes);
    }
    public async Task<Response<string>> ExcluirPublisherAsync(int id)
    {
      var response = new Response<string>();
      bool publisherExcluded = await _publisherRepository.ExcluirPublisherAsync(id);
      if (publisherExcluded)
      {
        response.Data = "Excluido com sucesso!";
        return response;
      }

      response.Data = "Erro ao excluir.";
      return response;
    }
  }
}
