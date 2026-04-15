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

        public async Task<List<GameModel>> BuscarGames()
        {
            return await _gameRepository.BuscarTodos();
        }

        public async Task<GameModel> BuscarGamePorId(long id)
        {
            return await _gameRepository.BuscarPorId(id);
        }


        public async Task<GameModel> CriarGame(GameModel game)
        {
            return await _gameRepository.Criar(game);
        }

        public async Task<bool> RemoverGame(long id)
        {
            return await _gameRepository.Deletar(id);
        }
        public async Task<GameModel> AtualizarGame(GameModel game)
        {
            return await _gameRepository.Atualizar(game);
        }
    }
}
