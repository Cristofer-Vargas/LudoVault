using LudoVault.Application.DTO.Responses;
using LudoVault.Application.Validations.Base;

namespace LudoVault.Application.Interfaces.Services
{
  public interface IPlatformServices
  {
    public Task<Response<List<PlatformResponse>>> BuscarPlataformas();
  }
}