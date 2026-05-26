using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;

namespace LudoVault.Repositories
{
  public class PlatformRepository(MysqlContext dbContext) : IPlatformRepository
  {
    private readonly MysqlContext _dbContext = dbContext;

    public async Task<PlatformModel>? BuscarPorId(int id)
    {
      try
      {
        return await _dbContext.Platforms.FindAsync(id);
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
