using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Services.Validations.Base;

namespace LudoVault.Services.Interfaces
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
