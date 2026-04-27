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
        private readonly IPublisherRepository _publisherRepository;

        public GameServices(IGameRepository gameRepo, IPublisherRepository publisherRepo)
        {
            _gameRepository = gameRepo;
            _publisherRepository = publisherRepo;
        }

        public async Task<List<GameResponse>> BuscarGames()
        {
            var gamesModel = await _gameRepository.BuscarTodos();
            if (gamesModel == null || gamesModel.Count == 0) 
                throw new Exception("Nenhum Jogo cadastrado!");

            return gamesModel
                .Select(game => GameMapper.ToResponse(game))
                .ToList();
        }

        public async Task<GameResponse> BuscarGamePorId(long id)
        {
            var gameModel = await _gameRepository.BuscarPorId(id);
            if (gameModel == null)
                throw new Exception("Nenhum Jogo encontrado!");

            return GameMapper.ToResponse(gameModel);
        }


        public async Task<GameResponse> CriarGame(GameRequest request)
        {
            var publisherModel = await _publisherRepository.BuscarPorId(request.PublisherId);

            var gameModel = GameMapper.ToModel(
                request, 
                publisherModel,
                request.PlatformIds,                
                request.GenreIds);

            await _gameRepository.Criar(gameModel);

            return GameMapper.ToResponse(gameModel);
        }

        public async Task<GameResponse> AtualizarGame(GameRequest game, long id)
        {
            var publisher = await _publisherRepository.BuscarPorId(game.PublisherId);

            var gameModel = GameMapper.ToModel(
                game,
                publisher,
                game.PlatformIds,
                game.GenreIds
                );

            var newGame = await _gameRepository.Atualizar(gameModel, id);
            return GameMapper.ToResponse(newGame);
        }

        public async Task<bool> RemoverGame(long id)
        {
            await _gameRepository.Deletar(id);
            return true;
        }

        public async Task<GameRatingListUsersResponse> BuscarRatingsPorIdGame(long id)
        {
            var gameRatings = await _gameRepository.BuscarRatings(id);
            if (gameRatings.Count == 0) throw new ArgumentException("Nenhuma avaliação encontrada!");

            var avgRatings = gameRatings.Select(gr => gr.Rating).Average();
            var totalRatings = gameRatings.Count;

            var gameRatingListUsersResponse = new GameRatingListUsersResponse
            {
                UsersRatings = gameRatings
                .Select(gr => GameRatingMapper.ToGameUserResponse(gr))
                .ToList(),
                AvgRatings = Math.Round(Convert.ToDouble(avgRatings), 1),
                TotalRatings = totalRatings
            };


            return gameRatingListUsersResponse;
        }
    }
}
