using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
    public class GameRepository : IGameRepository
    {
        public readonly MysqlContext _dbContext;
        public GameRepository(MysqlContext dbContext)
        {
            _dbContext = dbContext;
        }

       public async Task<GameModel> Criar(GameModel game)
        {
            throw new NotImplementedException();
        }

        public async Task<GameModel> Atualizar(GameModel game)
        {
            throw new NotImplementedException();
        }

        public async Task<GameModel> BuscarPorId(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GameModel>> BuscarTodos()
        {
            List<GameModel> games = await _dbContext.Games
            .Include(g => g.Publisher)
            .Include(g => g.GamePlatforms)
                .ThenInclude(gp => gp.Platform)
            .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
            .ToListAsync();
            return games;
        }

        public async Task<bool> Deletar(long id)
        {
            throw new NotImplementedException();
        }
    }
}
