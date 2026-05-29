using LudoVault.Data;
using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Repositories
{
  public class GameRepository(MysqlContext dbContext, ILogger<GameRepository> logger) : IGameRepository
  {
    private readonly MysqlContext _dbContext = dbContext;
    private readonly ILogger<GameRepository> _logger = logger;

    // Jogo
    public async Task<GameModel>? CriarAsync(GameModel game)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        await _dbContext.AddAsync(game);
        await _dbContext.SaveChangesAsync();

        foreach (var platform in game.GamePlatforms) { platform.GameId = game.Id; } // Para cada plataforma, e genero
        foreach (var genre in game.GameGenres) { genre.GameId = game.Id; }          // adiciona o ID do game para preencher no banco

        if (game.GamePlatforms.Count > 0) { _dbContext.GamePlatforms.UpdateRange(game.GamePlatforms); }
        if (game.GameGenres.Count > 0) { _dbContext.GameGenres.UpdateRange(game.GameGenres); }

        await _dbContext.SaveChangesAsync();

        await _dbContext.Database.CommitTransactionAsync();
        return await BuscarPorIdAsync(game.Id);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao criar jogo {GNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", game.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<GameModel>? AtualizarAsync(GameModel game)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
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
        await _dbContext.Database.CommitTransactionAsync();
        return await BuscarPorIdAsync(game.Id);
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao atualizar jogo {GID}:{GNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", game.Id, game.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return null;
      }
    }
    public async Task<GameModel>? BuscarPorIdAsync(int id)
    {
      try
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
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar jogo {GID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", id, e.Message, e.Source);
        return null;
      }
    }
    public async Task<List<GameModel>> BuscarTodosAsync()
    {
      try
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
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar todos os jogos no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", e.Message, e.Source);
        return new List<GameModel>();
      }
    }
    public async Task<bool> ExcluirAsync(GameModel game)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.Games.Remove(game);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return true;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao excluir jogo {GID}:{GNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", game.Id, game.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }
    public async Task<bool> AtualizarCaminhoDeImagem(GameModel game)
    {
      using var transaction = await _dbContext.Database.BeginTransactionAsync();

      try
      {
        _dbContext.Games
          .Update(game);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
        return true;
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao atualizar jogo {GID}:{GNAME} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", game.Id, game.Name, e.Message, e.Source);
        await _dbContext.Database.RollbackTransactionAsync();
        return false;
      }
    }

    // Avaliações de Jogo
    public async Task<List<RatingModel>> BuscarAvaliacoesAsync(int id)
    {
      try
      {
        return await _dbContext.Ratings
        .Include(gr => gr.Game)
        .Where(gr => gr.GameId == id)
        .Include(gr => gr.User)
        .AsSplitQuery()
        .ToListAsync();
      }
      catch (Exception e)
      {
        _logger.LogCritical("Erro ao buscar avaliações do jogo {GID} no banco de dados!\nMensagem: {EXC}\nLocal: {SOURCE}", id, e.Message, e.Source);
        return new List<RatingModel>();
      }

    }
  }
}
