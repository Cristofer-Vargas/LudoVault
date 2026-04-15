using LudoVault.Model;

namespace LudoVault.Services.Interfaces
{
    public interface IGameServices
    {
        public Task<GameModel> CriarGame(GameModel game);
        public Task<GameModel> AtualizarGame(GameModel game);
        public Task<List<GameModel>> BuscarGames();
        public Task<GameModel> BuscarGamePorId(long id);
        public Task<bool> RemoverGame(long id);
    }
}
