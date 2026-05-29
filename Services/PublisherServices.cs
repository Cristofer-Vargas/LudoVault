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
      var inputErrors = new List<Report>();
      var response = new Response<PublisherResponse>(inputErrors);
      if (string.IsNullOrWhiteSpace(publisher.Name))
        inputErrors.Add(Report.Create("Nome de publisher deve ser preenchido corretamente!", 400));
      if (inputErrors.Count > 0)
        return response;

      var publisherModel = PublisherMapper.ToModel(publisher);
      var publisherCreated = await _publisherRepository.CriarAsync(publisherModel);
      if (publisherCreated == null)
      {
        _logger.LogError("Erro interno ao criar publisher {PNAME}.", publisher.Name);
        response.Report.Add(Report.Create($"Erro ao criar publisher {publisher.Name}.", 500));
        return response;
      }

      response.Data = PublisherMapper.ToResponse(
              publisherCreated,
              publisherCreated.Games.Select(p => GameMapper.ToResponse(p)).ToList());

      _logger.LogInformation("Publisher {PID}:{PNAME} criada com sucesso.", publisherCreated.Id, publisherCreated.Name);
      return response;
    }
    public async Task<Response<PublisherResponse>> AtualizarPublisherAsync(PublisherRequest publisher, int id)
    {
      var response = new Response<PublisherResponse>();
      var pub = await _publisherRepository.BuscarPorIdAsync(id);
      if (pub == null)
      {
        response.Report.Add(Report.Create("Publisher não encontrada!", 404));
        return response;
      }

      var publisherModel = PublisherMapper.ToModel(publisher);
      publisherModel.Id = pub.Id;
      var updatedPublisher = await _publisherRepository.AtualizarAsync(publisherModel);
      if (updatedPublisher == null)
      {
        _logger.LogError("Erro interno ao atualizar publisher {PID}:{PNAME}.", publisherModel.Id, publisherModel.Name);
        response.Report.Add(Report.Create($"Erro ao atualizar publisher {publisher.Name}.", 500));
        return response;
      }

      response.Data = PublisherMapper.ToResponse(
              updatedPublisher,
              updatedPublisher.Games.Select(GameMapper.ToResponse).ToList());

      _logger.LogInformation("Publisher {PID}:{POLDNAME} atualizada para {PNEWNAME}", updatedPublisher.Id, pub.Name, updatedPublisher.Name);
      return response;
    }
    public async Task<Response<List<PublisherResponse>>> BuscarTodasPublishersAsync()
    {
      var response = new Response<List<PublisherResponse>>();
      List<PublisherModel> pubModelList = await _publisherRepository.BuscarTodosAsync();

      response.Data = pubModelList.Select(publisher => PublisherMapper.ToResponse(
              publisher,
              publisher.Games.Select(GameMapper.ToResponse)
      .ToList())).ToList();

      return response;
    }
    public async Task<Response<PublisherResponse>> BuscarPublisherPorIdAsync(int id)
    {
      var response = new Response<PublisherResponse>();

      var publisherModel = await _publisherRepository.BuscarPorIdAsync(id);
      if (publisherModel == null)
      {
        response.Report.Add(Report.Create("Publisher não encontrada!", 404));
        return response;
      }

      response.Data = PublisherMapper.ToResponse(
              publisherModel,
              publisherModel.Games.Select(GameMapper.ToResponse).ToList()
              );

      return response;
    }
    public async Task<Response<string>> ExcluirPublisherAsync(int id)
    {
      var response = new Response<string>();
      var pub = await _publisherRepository.BuscarPorIdAsync(id);
      if (pub == null)
      {
        response.Report.Add(Report.Create("Publisher não encontrada!", 404));
        return response;
      }

      bool publisherExcluded = await _publisherRepository.ExcluirAsync(pub);
      if (!publisherExcluded)
      {
        _logger.LogError("Erro ao excluir publisher {PID}:{PNAME}.", pub.Id, pub.Name);
        response.Report.Add(Report.Create($"Erro interno ao excluir {pub.Name}.", 500));
        return response;
      }

      response.Data = $"Publisher {pub.Name} excluido com sucesso.";
      _logger.LogInformation("Publisher {PID}:{PNAME} excluida com sucesso.", pub.Id, pub.Name);
      return response;
    }
  }
}
