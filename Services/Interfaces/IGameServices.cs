using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Interfaces
{
    public interface IGameServices
    {
        public Task<GameResponse> CriarGame(GameRequest game);
        public Task<GameResponse> AtualizarGame(GameRequest game, int id);
        public Task<List<GameResponse>> BuscarGames();
        public Task<GameResponse> BuscarGamePorId(int id);
        public Task<bool> RemoverGame(int id);
        public Task<GameRatingListUsersResponse> BuscarRatingsPorIdGame(int id);
    }
}
