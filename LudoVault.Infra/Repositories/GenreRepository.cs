using LudoVault.Infra.Data;
using LudoVault.Domain.Model;
using LudoVault.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Infra.Repositories
{
  public class GenreRepository(MysqlContext dbContext, ILogger<GenreRepository> logger) : IGenreRepository
  {
    private readonly MysqlContext _dbContext = dbContext;
    private readonly ILogger<GenreRepository> _logger = logger;

    public async Task<GenreModel>? BuscarPorId(int id)
    {
      try
      {
        return await _dbContext.Genres.FindAsync(id);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar Gênerp {GID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", id, e.Message, e.Source);
        return null;
      }
    }
  }
}
