using LudoVault.Application.DTO.Responses;
using LudoVault.Application.Validations.Base;

namespace LudoVault.Application.Interfaces.Services
{
  public interface IGenreServices
  {
    public Task<Response<List<GenreResponse>>> BuscarGeneros();
  }
}