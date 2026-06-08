using LudoVault.Domain.Model;

namespace LudoVault.Domain.Interfaces.Repositories
{
  public interface IGenreRepository
  {
    public Task<GenreModel>? BuscarPorId(int id);
  }
}
