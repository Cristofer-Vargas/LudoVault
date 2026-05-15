using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Services.Validations.Base;

namespace LudoVault.Services.Interfaces
{
  public interface IGameServices
  {
    // Jogo
    public Task<Response<GameResponse>> CriarGameAsync(GameRequest game);
    public Task<Response<GameResponse>> AtualizarGameAsync(GameRequest game, int id);
    public Task<Response<List<GameResponse>>> BuscarTodosGamesAsync();
    public Task<Response<GameResponse>> BuscarGamePorIdAsync(int id);
    public Task<Response<string>> RemoverGameAsync(int id);
    public Task<Response<string>> AdicionarImagemDeCapaAsync(IFormFile image, int gameId);
    public Task<Response<string>> RemoverImagemDeCapaAsync(int gameId);

    // Avaliações de Jogo
    public Task<Response<RatingListUsersResponse>> BuscarAvaliacoesPorJogoAsync(int id);
  }
}
