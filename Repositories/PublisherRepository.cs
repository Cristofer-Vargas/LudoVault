using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
  public class PublisherRepository(MysqlContext dbContext, ILogger<PublisherRepository> logger) : IPublisherRepository
  {
    private readonly MysqlContext _dbContext = dbContext;
    private readonly ILogger<PublisherRepository> _logger = logger;

    // Desenvolvedora
    public async Task<PublisherModel>? CriarAsync(PublisherModel publisher)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        await _dbContext.Publishers.AddAsync(publisher);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return publisher;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao criar publisher {PNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", publisher.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<PublisherModel>? AtualizarAsync(PublisherModel publisher)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.Publishers.Update(publisher);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return publisher;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao atualizar publisher {PID}:{PNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", publisher.Id, publisher.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<PublisherModel>? BuscarPorIdAsync(int id)
    {
      try
      {
        return await _dbContext.Publishers
              .Include(p => p.Games)
              .FirstOrDefaultAsync(p => p.Id == id);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar a publisher {PID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", id, e.Message, e.Source);
        return null;
      }
    }
    public async Task<List<PublisherModel>> BuscarTodosAsync()
    {
      try
      {
        return await _dbContext.Publishers
            .Include(p => p.Games)
            .ToListAsync();
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar todas as publishers no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", e.Message, e.Source);
        return new List<PublisherModel>();
      }
    }
    public async Task<bool> ExcluirAsync(PublisherModel publisher)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.Publishers.Remove(publisher);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return true;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao excluir publisher {PID}:{PNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", publisher.Id, publisher.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }
  }
}
