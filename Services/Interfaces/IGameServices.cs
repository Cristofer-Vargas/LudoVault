using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;

namespace LudoVault.Services.Interfaces
{
  public interface IGameServices
  {
    // Jogo
    public Task<GameResponse> CriarGameAsync(GameRequest game);
    public Task<GameResponse> AtualizarGameAsync(GameRequest game, int id);
    public Task<List<GameResponse>> BuscarTodosGamesAsync();
    public Task<GameResponse> BuscarGamePorIdAsync(int id);
    public Task<bool> RemoverGameAsync(int id);
    public Task<string> AdicionarImagemDeCapaAsync(IFormFile image, int gameId);
    public Task<bool> RemoverImagemDeCapaAsync(int gameId);

    // Avaliações de Jogo
    public Task<GameRatingListUsersResponse> BuscarAvaliacoesPorJogoAsync(int id);
  }
}
