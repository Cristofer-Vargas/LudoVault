using LudoVault.Application.DTO.Requests;
using LudoVault.Application.DTO.Responses;
using LudoVault.Application.Validations.Base;

namespace LudoVault.Application.Interfaces.Services
{
  public interface IPublisherServices
  {
    // Desenvolvedora
    public Task<Response<PublisherResponse>> CriarPublisherAsync(PublisherRequest publisher);
    public Task<Response<PublisherResponse>> AtualizarPublisherAsync(PublisherRequest publisher, int id);
    public Task<Response<List<PublisherResponse>>> BuscarTodasPublishersAsync();
    public Task<Response<PublisherResponse>> BuscarPublisherPorIdAsync(int id);
    public Task<Response<string>> ExcluirPublisherAsync(int id);
  }
}
