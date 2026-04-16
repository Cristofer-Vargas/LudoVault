using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Interfaces
{
    public interface IGameServices
    {
        public Task<GameResponse> CriarGame(GameRequest game);
        public Task<GameResponse> AtualizarGame(GameRequest game);
        public Task<List<GameResponse>> BuscarGames();
        public Task<GameResponse> BuscarGamePorId(long id);
        public Task<bool> RemoverGame(long id);
    }
}
