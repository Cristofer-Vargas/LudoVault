using LudoVault.Model;

namespace LudoVault.Services.Interfaces
{
    public interface IGameServices
    {
        public GameModel CriarGame(GameModel game);
        public GameModel AtualizarGame(GameModel game);
        public List<GameModel> BuscarGames();
        public GameModel BuscarGamePorId(long id);
        public bool RemoverGame(long id);
    }
}
