using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
  public interface IGameRepository
  {
    // Jogo
    public Task<GameModel>? CriarGameAsync(GameModel game);
    public Task<List<GameModel>> BuscarTodosGamesAsync();
    public Task<GameModel>? BuscarGamePorIdAsync(int id);
    public Task<GameModel>? AtualizarGameAsync(GameModel game);
    public Task<bool> DeletarGameAsync(GameModel game);
    public Task<bool> GameExisteAsync(int gameId);
    public Task<bool> AtualizarCaminhoDeImagemEmGame(GameModel game);

    // Avaliações de Jogo
    public Task<List<RatingModel>> BuscarAvaliacoesDoJogoAsync(int id);
  }
}
