using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;

namespace LudoVault.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        public readonly MysqlContext _dbContext;
        public PublisherRepository(MysqlContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PublisherModel>> BuscarTodos()
        {
            throw new NotImplementedException();
        }
    }
}
