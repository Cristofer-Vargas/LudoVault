using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
  public class GenreRepository(MysqlContext dbContext) : IGenreRepository
  {
    private readonly MysqlContext _dbContext = dbContext;

    public async Task<GenreModel>? BuscarPorId(int id)
    {
      try
      {
        return await _dbContext.Genres.FindAsync(id);
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
