using LudoVault.Domain.Model;

namespace LudoVault.Domain.Interfaces.Repositories
{
  public interface IGenreRepository
  {
    public Task<List<GenreModel>> BuscarTodos();
    public Task<GenreModel>? BuscarPorId(int id);
  }
}
