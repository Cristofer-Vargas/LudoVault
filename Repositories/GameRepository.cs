using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
  public class GameRepository(MysqlContext dbContext) : IGameRepository
  {
    public readonly MysqlContext _dbContext = dbContext;

    // Jogo
    public async Task<GameModel>? CriarGameAsync(GameModel game)
    {
      await _dbContext.AddAsync(game);
      await _dbContext.SaveChangesAsync();

      foreach (var platform in game.GamePlatforms) { platform.GameId = game.Id; } // Para cada plataforma, e genero
      foreach (var genre in game.GameGenres) { genre.GameId = game.Id; }          // adiciona o ID do game para preencher no banco

      if (game.GamePlatforms.Count > 0) { _dbContext.GamePlatforms.UpdateRange(game.GamePlatforms); }
      if (game.GameGenres.Count > 0) { _dbContext.GameGenres.UpdateRange(game.GameGenres); }

      await _dbContext.SaveChangesAsync();

      return await BuscarGamePorIdAsync(game.Id);
    }
    public async Task<GameModel>? AtualizarGameAsync(GameModel game)
    {
      // Deleta os GamePlatforms e GameGenres ANTIGOS pelo GameId
      var oldPlatforms = await _dbContext.GamePlatforms
          .Where(gp => gp.GameId == game.Id)
          .ToListAsync();
      _dbContext.GamePlatforms.RemoveRange(oldPlatforms);

      var oldGenres = await _dbContext.GameGenres
          .Where(gg => gg.GameId == game.Id)
          .ToListAsync();
      _dbContext.GameGenres.RemoveRange(oldGenres);

      // Preenche os IDs das NOVAS relações
      foreach (var platform in game.GamePlatforms)
        platform.GameId = game.Id;
      foreach (var genre in game.GameGenres)
        genre.GameId = game.Id;

      // Atualiza o Game e adiciona as novas relações
      _dbContext.Games.Update(game);
      if (game.GamePlatforms.Count > 0)
        _dbContext.GamePlatforms.AddRange(game.GamePlatforms);
      if (game.GameGenres.Count > 0)
        _dbContext.GameGenres.AddRange(game.GameGenres);

      await _dbContext.SaveChangesAsync();
      return await BuscarGamePorIdAsync(game.Id);
    }
    public async Task<GameModel>? BuscarGamePorIdAsync(int id)
    {
      var game = await _dbContext.Games
      .Where(g => g.Id == id)
      .Include(g => g.Publisher)
      .Include(g => g.GamePlatforms)
              .ThenInclude(gp => gp.Platform)
      .Include(g => g.GameGenres)
              .ThenInclude(gg => gg.Genre)
      .AsSplitQuery()
      .FirstOrDefaultAsync(g => g.Id == id);

      return game;
    }
    public async Task<List<GameModel>> BuscarTodosGamesAsync()
    {
      List<GameModel> games = await _dbContext.Games
      .Include(g => g.Publisher)
      .Include(g => g.GamePlatforms)
              .ThenInclude(gp => gp.Platform)
      .Include(g => g.GameGenres)
              .ThenInclude(gg => gg.Genre)
      .AsSplitQuery()
      .ToListAsync();
      return games;
    }
    public async Task<bool> DeletarGameAsync(GameModel game)
    {
      _dbContext.Games.Remove(game);
      await _dbContext.SaveChangesAsync();

      return true;
    }
    public async Task<bool> GameExisteAsync(int gameId)
    {
      var gameExist = await _dbContext.Games
              .FirstOrDefaultAsync(g => g.Id == gameId);

      if (gameExist == null) return false;
      return true;
    }
    public async Task<bool> AtualizarCaminhoDeImagemEmGame(GameModel game)
    {
      _dbContext.Games
        .Update(game);

      await _dbContext.SaveChangesAsync();

      return true;
    }

    // Avaliações de Jogo
    public async Task<List<RatingModel>> BuscarAvaliacoesDoJogoAsync(int id)
    {
      List<RatingModel> gameRatings = await _dbContext.Ratings
              .Include(gr => gr.Game)
              .Where(gr => gr.GameId == id)
              .Include(gr => gr.User)
              .AsSplitQuery()
              .ToListAsync();

      return gameRatings;
    }
  }
}
