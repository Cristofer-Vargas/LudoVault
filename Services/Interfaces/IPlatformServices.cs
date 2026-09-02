using LudoVault.DTO.Responses;
using LudoVault.Validations.Base;

namespace LudoVault.Services.Interfaces
{
  public interface IPlatformServices
  {
    public Task<Response<List<PlatformResponse>>> BuscarPlataformas();
  }
}