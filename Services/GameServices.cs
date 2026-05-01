﻿﻿﻿using LudoVault.Repositories.Interfaces;
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

        public async Task<List<GameResponse>> BuscarTodosGamesAsync()
        {
            var gamesModel = await _gameRepository.BuscarTodosGamesAsync();
            if (gamesModel == null || gamesModel.Count == 0) 
                throw new Exception("Nenhum Jogo cadastrado!");

            return gamesModel
                .Select(game => GameMapper.ToResponse(game))
                .ToList();
        }

        public async Task<GameResponse> BuscarGamePorIdAsync(int id)
        {
            var gameModel = await _gameRepository.BuscarGamePorIdAsync(id);
            if (gameModel == null)
                throw new Exception("Nenhum Jogo encontrado!");

            return GameMapper.ToResponse(gameModel);
        }


        public async Task<GameResponse> CriarGameAsync(GameRequest request)
        {
            var publisherModel = await _publisherRepository.BuscarPublisherPorIdAsync(request.PublisherId);

            var gameModel = GameMapper.ToModel(
                request, 
                publisherModel,
                request.PlatformIds,                
                request.GenreIds);

            await _gameRepository.CriarGameAsync(gameModel);

            return GameMapper.ToResponse(gameModel);
        }

        public async Task<GameResponse> AtualizarGameAsync(GameRequest game, int id)
        {
            var publisher = await _publisherRepository.BuscarPublisherPorIdAsync(game.PublisherId);

            var gameModel = GameMapper.ToModel(
                game,
                publisher,
                game.PlatformIds,
                game.GenreIds
                );

            var newGame = await _gameRepository.AtualizarGameAsync(gameModel, id);
            return GameMapper.ToResponse(newGame);
        }

        public async Task<bool> RemoverGameAsync(int id)
        {
            await _gameRepository.DeletarGameAsync(id);
            return true;
        }

        public async Task<GameRatingListUsersResponse> BuscarAvaliacoesPorJogoAsync(int id)
        {
            var gameRatings = await _gameRepository.BuscarAvaliacoesDoJogoAsync(id);
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
