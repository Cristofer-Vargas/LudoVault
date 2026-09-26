using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
  public class DeveloperRepository(MysqlContext dbContext, ILogger<DeveloperRepository> logger) : IDeveloperRepository
  {
    private readonly MysqlContext _dbContext = dbContext;
    private readonly ILogger<DeveloperRepository> _logger = logger;

    // Desenvolvedora
    public async Task<DeveloperModel>? CriarAsync(DeveloperModel developer)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        await _dbContext.Developers.AddAsync(developer);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return developer;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao criar developer {PNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", developer.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<DeveloperModel>? AtualizarAsync(DeveloperModel developer)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        var trackedEntity = _dbContext.Developers.Local.FirstOrDefault(p => p.Id == developer.Id);
        if (trackedEntity != null)
        {
          _dbContext.Entry(trackedEntity).State = EntityState.Detached;
        }

        _dbContext.Developers.Update(developer);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return developer;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao atualizar developer {PID}:{PNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", developer.Id, developer.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<DeveloperModel>? BuscarPorIdAsync(int id)
    {
      try
      {
        return await _dbContext.Developers
              .Include(d => d.DeveloperGames)
              .FirstOrDefaultAsync(p => p.Id == id);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar a developer {PID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", id, e.Message, e.Source);
        return null;
      }
    }
    public async Task<List<DeveloperModel>> BuscarTodosAsync()
    {
      try
      {
        return await _dbContext.Developers
            .Include(d => d.DeveloperGames)
            .ThenInclude(dg => dg.Game)
            .ToListAsync();
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar todas as developers no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", e.Message, e.Source);
        return new List<DeveloperModel>();
      }
    }
    public async Task<bool> ExcluirAsync(DeveloperModel developer)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.Developers.Remove(developer);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return true;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao excluir developer {PID}:{PNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", developer.Id, developer.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }
  }
}
