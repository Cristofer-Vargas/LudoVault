using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Validations.Base;

namespace LudoVault.Services
{
  public class PublisherServices(IPublisherRepository publisherReposiroty, ILogger<PublisherServices> logger) : IPublisherServices
  {
    private readonly IPublisherRepository _publisherRepository = publisherReposiroty;

    private readonly ILogger<PublisherServices> _logger = logger;

    public async Task<Response<PublisherResponse>> CriarPublisherAsync(PublisherRequest publisher)
    {
      var publisherModel = await _publisherRepository.CriarPublisherAsync(PublisherMapper.ToModel(publisher));
      var pubRes = PublisherMapper.ToResponse(
              publisherModel,
              publisherModel.Games.Select(GameMapper.ToResponse).ToList());

      _logger.LogInformation("Publisher {PNAME} criada com sucesso.", publisherModel.Name);
      return Response.Ok(pubRes);
    }
    public async Task<Response<PublisherResponse>> AtualizarPublisherAsync(PublisherRequest publisher, int id)
    {
      var response = new Response<PublisherResponse>();
      var pub = await _publisherRepository.BuscarPublisherPorIdAsync(id);
      if (pub == null)
      {
        response.Report.Add(Report.Create("Publisher não encontrada!", 400));
        return response;
      }

      var publisherModel = PublisherMapper.ToModel(publisher);
      publisherModel.Id = id;
      var updatedPublisher = await _publisherRepository.AtualizarPublisherAsync(publisherModel);

      var pubRes = PublisherMapper.ToResponse(
              updatedPublisher,
              updatedPublisher.Games.Select(GameMapper.ToResponse).ToList());

      _logger.LogInformation("Publisher atualizada de {POLDNAME} para {PNEWNAME}", pub.Name, updatedPublisher.Name);
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
      var pub = await _publisherRepository.BuscarPublisherPorIdAsync(id);
      if (pub == null)
      {
        response.Report.Add(Report.Create("Publisher não encontrada!", 400));
        return response;
      }

      bool publisherExcluded = await _publisherRepository.ExcluirPublisherAsync(id);
      if (publisherExcluded)
      {
        response.Data = "Excluido com sucesso!";
        return response;
      }

      _logger.LogError("Erro ao excluir publisher {PID}:{PNAME}.", pub.Id, pub.Name);
      response.Data = "Erro ao excluir.";
      return response;
    }
  }
}
