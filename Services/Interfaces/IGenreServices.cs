using LudoVault.DTO.Responses;
using LudoVault.Validations.Base;

namespace LudoVault.Services.Interfaces
{
  public interface IGenreServices
  {
    public Task<Response<List<GenreResponse>>> BuscarGeneros();
  }
}