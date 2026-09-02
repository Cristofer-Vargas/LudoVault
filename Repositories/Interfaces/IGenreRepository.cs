using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
  public interface IGenreRepository
  {
    public Task<List<GenreModel>> BuscarTodos();
    public Task<GenreModel>? BuscarPorId(int id);
  }
}
