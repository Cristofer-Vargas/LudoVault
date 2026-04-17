using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly MysqlContext _dbContext;
        public PublisherRepository(MysqlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PublisherModel> Criar(PublisherModel publisher)
        {
            await _dbContext.Publishers.AddAsync(publisher);
            await _dbContext.SaveChangesAsync();
            return publisher;
        }

        public async Task<PublisherModel> Atualizar(PublisherModel publisher)
        {
            var currentPublisher = await _dbContext.Publishers.FindAsync(publisher.Id);
            if (currentPublisher == null)
                throw new KeyNotFoundException($"Publisher não encontrado.");

            currentPublisher.Name = publisher.Name;

            _dbContext.Publishers.Update(currentPublisher);
            await _dbContext.SaveChangesAsync();
            return currentPublisher;
        }

        public async Task<PublisherModel> BuscarPorId(long id)
        {
            var publisher = await _dbContext.Publishers
                .Include(p => p.Games)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (publisher == null)
                throw new KeyNotFoundException($"Não foi encontrado a publisher especíofica!");

            return publisher;
        }

        public async Task<List<PublisherModel>> BuscarTodos()
        {
            List<PublisherModel> publishers = await _dbContext.Publishers
                .FromSql($"SELECT * FROM publisher")
                .Include(p => p.Games)
                .ToListAsync();

            if (publishers.Count == 0) throw new Exception("Nenhuma publisher Cadastrada!");

            return publishers;
        }

        public async Task<bool> Excluir(long id)
        {
            var publisher = await BuscarPorId(id);
            _dbContext.Publishers.Remove(publisher);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
