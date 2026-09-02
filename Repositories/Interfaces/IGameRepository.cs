using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
  public interface IGameRepository
  {
    // Jogo
    public Task<GameModel>? CriarAsync(GameModel game);
    public Task<GameModel>? AtualizarAsync(GameModel game);
    public Task<bool> AtualizarCaminhoDeImagem(GameModel game);
    public Task<List<GameModel>> BuscarTodosAsync();
    public Task<GameModel>? BuscarPorIdAsync(int id);
    public Task<bool> ExcluirAsync(GameModel game);

    // Avaliações de Jogo
    public Task<List<RatingModel>> BuscarAvaliacoesAsync(int id);
  }
}
