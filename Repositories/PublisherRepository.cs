using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
  public class PublisherRepository(MysqlContext dbContext) : IPublisherRepository
  {
    private readonly MysqlContext _dbContext = dbContext;

    // Desenvolvedora
    public async Task<PublisherModel>? CriarAsync(PublisherModel publisher)
    {
      await _dbContext.Publishers.AddAsync(publisher);
      await _dbContext.SaveChangesAsync();
      return publisher;
    }
    public async Task<PublisherModel>? AtualizarAsync(PublisherModel publisher)
    {
      var currentPublisher = await _dbContext.Publishers.FindAsync(publisher.Id);
      if (currentPublisher == null)
        throw new KeyNotFoundException($"Publisher não encontrado.");

      currentPublisher.Name = publisher.Name;

      _dbContext.Publishers.Update(currentPublisher);
      await _dbContext.SaveChangesAsync();
      return currentPublisher;
    }
    public async Task<PublisherModel>? BuscarPorIdAsync(int id)
    {
      var publisher = await _dbContext.Publishers
              .Include(p => p.Games)
              .FirstOrDefaultAsync(p => p.Id == id);
      return publisher;
    }
    public async Task<List<PublisherModel>> BuscarTodosAsync()
    {
      List<PublisherModel> publishers = await _dbContext.Publishers
              .Include(p => p.Games)
              .ToListAsync();

      if (publishers.Count == 0) throw new Exception("Nenhuma publisher Cadastrada!");

      return publishers;
    }
    public async Task<bool> ExcluirAsync(PublisherModel publisher)
    {
      _dbContext.Publishers.Remove(publisher);
      await _dbContext.SaveChangesAsync();
      return true;
    }
  }
}
