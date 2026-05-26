using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
  public interface IGenreRepository
  {
    public Task<GenreModel>? BuscarPorId(int id);
  }
}
