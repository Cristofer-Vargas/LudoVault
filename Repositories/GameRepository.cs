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
            // Salva o Game para gerar o ID
            await _dbContext.AddAsync(game);
            await _dbContext.SaveChangesAsync();

            foreach (var platform in game.GamePlatforms) { platform.GameId = game.Id; } // Para cada plataforma, e genero
            foreach (var genre in game.GameGenres) { genre.GameId = game.Id; }          // adiciona o ID do game para preencher no banco

            // Adiciona explicitamente ao contexto
            if (game.GamePlatforms.Count > 0) { _dbContext.GamePlatforms.UpdateRange(game.GamePlatforms); }
            if (game.GameGenres.Count > 0) { _dbContext.GameGenres.UpdateRange(game.GameGenres); }

            await _dbContext.SaveChangesAsync();

            return await BuscarPorId(game.Id) ?? throw new Exception("Erro ao recuperar o Jogo recém criado!");
        }

        public async Task<GameModel> Atualizar(GameModel game)
        {
            throw new NotImplementedException();
        }

        public async Task<GameModel> BuscarPorId(long id)
        {
            var game = await _dbContext.Games
            .Where(g => g.Id == id)
            .Include(g => g.Publisher)
            .Include(g => g.GamePlatforms)
                .ThenInclude(gp => gp.Platform)
            .Include(g => g.GameGenres)
                .ThenInclude(gg => gg.Genre)
            .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null) 
                throw new ArgumentException("Nenhum Jogo encontrado!");

            return game;
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
