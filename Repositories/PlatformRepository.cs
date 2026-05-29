using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;

namespace LudoVault.Repositories
{
  public class PlatformRepository(MysqlContext dbContext, ILogger<PlatformRepository> logger) : IPlatformRepository
  {
    private readonly MysqlContext _dbContext = dbContext;
    private readonly ILogger<PlatformRepository> _logger = logger;

    public async Task<PlatformModel>? BuscarPorId(int id)
    {
      try
      {
        return await _dbContext.Platforms.FindAsync(id);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar plataforma {PID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", id, e.Message, e.Source);
        return null;
      }

    }
  }
}
