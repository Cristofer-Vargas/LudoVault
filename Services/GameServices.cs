using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services
{
    public class GameServices : IGameServices
    {
        private readonly IGameRepository _gameRepository;

        public GameServices(IGameRepository gameRepo)
        {
            _gameRepository = gameRepo;
        }

        public async Task<List<GameResponse>> BuscarGames()
        {
            var gamesModel = await _gameRepository.BuscarTodos();
            if (gamesModel == null || gamesModel.Count() == 0) throw new Exception("Nenhum Jogo cadastrado!");

            List<GameResponse> gameRes = [];
            foreach (var game in gamesModel)
            {
                gameRes.Add(GameMapper.ToResponse(game));
            }
            return gameRes;
        }

        public async Task<GameResponse> BuscarGamePorId(long id)
        {
            throw new NotImplementedException();
        }


        public async Task<GameResponse> CriarGame(GameRequest game)
        {
            throw new NotImplementedException();
        }

        public async Task<GameResponse> AtualizarGame(GameRequest game)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoverGame(long id)
        {
            throw new NotImplementedException();
        }
    }
}
