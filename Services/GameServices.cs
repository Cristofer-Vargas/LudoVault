using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;

namespace LudoVault.Services
{
    public class GameServices : IGameServices
    {
        private readonly IGameReposiroty _gameRepository;

        public GameServices(IGameReposiroty gameRepo)
        {
            _gameRepository = gameRepo;
        }

        public List<GameModel> BuscarGames()
        {
            return _gameRepository.BuscarTodos();
        }

        public GameModel BuscarGamePorId(long id)
        {
            return _gameRepository.BuscarPorId(id);
        }


        public GameModel CriarGame(GameModel game)
        {
            return _gameRepository.Criar(game);
        }

        public bool RemoverGame(long id)
        {
            return _gameRepository.Deletar(id);
        }
        public GameModel AtualizarGame(GameModel game)
        {
            return _gameRepository.Atualizar(game);
        }
    }
}
