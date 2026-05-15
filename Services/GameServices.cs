﻿using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Validations.Base;

namespace LudoVault.Services
{
  public class GameServices(IGameRepository gameRepo, IPublisherRepository publisherRepo, 
    IImageServices imageServices, ISystemServices sistema) : IGameServices
  {
    private readonly IGameRepository _gameRepository = gameRepo;
    private readonly IPublisherRepository _publisherRepository = publisherRepo;
    private readonly IImageServices _imageServices = imageServices;
    private readonly ISystemServices _sistema = sistema;

    // Jogo
    public async Task<Response<List<GameResponse>>> BuscarTodosGamesAsync()
    {
      var response = new Response<List<GameResponse>>();

      var gamesModel = await _gameRepository.BuscarTodosGamesAsync();
      if (gamesModel == null) return Response.Ok(new List<GameResponse>());
        
      var games =  gamesModel.Select(game => GameMapper.ToResponse(game)).ToList();

      return Response.Ok(games);
    }
    public async Task<Response<GameResponse>> BuscarGamePorIdAsync(int id)
    {
      var response = new Response<GameResponse>();

      var gameModel = await _gameRepository.BuscarGamePorIdAsync(id);

      if (gameModel == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;

      }

      response.Data = GameMapper.ToResponse(gameModel);
      return response;
    }
    public async Task<Response<GameResponse>> CriarGameAsync(GameRequest request)
    {
      var response = new Response<GameResponse>();

      var publisherModel = await _publisherRepository.BuscarPublisherPorIdAsync(request.PublisherId);

        if (publisherModel == null)
        {
          response.Report.Add(Report.Create("Não foi possivel encontrar a Publisher responsável."));
          return response;
        }

        if (string.IsNullOrWhiteSpace(request.ImageUrl))
        {
        request.ImageUrl = _sistema.CaminhoGameDefaultImage();
        }

      var gameModel = GameMapper.ToModel(
              request,
              publisherModel,
              request.PlatformIds,
              request.GenreIds
              );


      var game = await _gameRepository.CriarGameAsync(gameModel);
      return Response.Ok(GameMapper.ToResponse(game));
    }
    public async Task<Response<GameResponse>> AtualizarGameAsync(GameRequest game, int id)
    {
      var response = new Response<GameResponse>();

      var currentGame = await _gameRepository.BuscarGamePorIdAsync(id);
        if (currentGame == null)
        {
          response.Report.Add(Report.Create("Jogo não encontrado!"));
          return response;
        }

      var publisher = await _publisherRepository.BuscarPublisherPorIdAsync(game.PublisherId);
        if (publisher == null)
        {
          response.Report.Add(Report.Create("Não foi possivel encontrar a Publisher responsável."));
          return response;
        }

        if (string.IsNullOrWhiteSpace(game.ImageUrl))
        {
          game.ImageUrl = _sistema.CaminhoGameDefaultImage();
        }

      currentGame.Name = game.Name;
      currentGame.Description = game.Description;
      currentGame.ImageUrl = game.ImageUrl;
      currentGame.PublisherId = game.PublisherId;
      currentGame.Publisher = publisher;
      
      currentGame.GamePlatforms = game.PlatformIds.Select(id => PlatformMapper.ToGamePlatformModel(id)).ToList();
      currentGame.GameGenres = game.GenreIds.Select(id => GenreMapper.ToGameGenreModel(id)).ToList();

      var newGame = await _gameRepository.AtualizarGameAsync(currentGame);
      if (newGame == null)
      {
        response.Report.Add(Report.Create("Não foi possivel encontrar jogo recem criado!"));
        return response;
      }

      return Response.Ok(GameMapper.ToResponse(newGame));
    }
    public async Task<Response<string>> RemoverGameAsync(int gameId)
    {
      var response = new Response<string>();

      var game = await _gameRepository.BuscarGamePorIdAsync(gameId);

      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      if (game.ImageUrl != _sistema.CaminhoGameDefaultImage())    // Garantir que se for imagem dafult, não o exclua do servidor
      {
        if (!_imageServices.ExcluirImagemAsset(game.ImageUrl))
        {
          response.Report.Add(Report.Create("Erro ao excluir imagem do servidor!"));
          return response;
        }
      }

      var deletado = await _gameRepository.DeletarGameAsync(gameId);
      if (!deletado)
      {
          response.Report.Add(Report.Create("Erro ao excluir jogo do banco de dados."));
          return response;
      }

      return Response.Ok("Jogo excluído com sucesso!");
    }
    public async Task<Response<string>> AdicionarImagemDeCapaAsync(IFormFile image, int gameId)
    {
      var response = new Response<string>();

      var game = await _gameRepository.BuscarGamePorIdAsync(gameId);

      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      if (game.ImageUrl != _sistema.CaminhoGameDefaultImage())
      {
        _imageServices.ExcluirImagemAsset(game.ImageUrl);
      }

      var caminhoImg = await _imageServices.ConverteParaWebpESalvaImagem(image, "games");
      game.ImageUrl = caminhoImg;
      await _gameRepository.AtualizarCaminhoDeImagemEmGame(game);

      response.Data = caminhoImg;

      return response;
    }
    public async Task<Response<string>> RemoverImagemDeCapaAsync(int gameId)
    {
      var response = new Response<string>();

      var game = await _gameRepository.BuscarGamePorIdAsync(gameId);

      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      if (!_imageServices.ExcluirImagemAsset(game.ImageUrl ?? ""))
      {
        response.Report.Add(Report.Create("Não foi possivel excluir essa imagem!"));
        return response;
      }
      
      game.ImageUrl = _sistema.CaminhoGameDefaultImage();
      await _gameRepository.AtualizarCaminhoDeImagemEmGame(game);

      response.Data = "Imagem removida com sucesso!";

      return response;
    }

    // Avaliações de Jogo
    public async Task<Response<RatingListUsersResponse>> BuscarAvaliacoesPorJogoAsync(int id)
    {
      var response = new Response<RatingListUsersResponse>();
      var game = await _gameRepository.BuscarGamePorIdAsync(id);

      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      var gameRatings = await _gameRepository.BuscarAvaliacoesDoJogoAsync(id);
      if (gameRatings.Count == 0)
      {
        response.Report.Add(Report.Create("Nenhuma avaliação encontrada!"));
        return response;
      }

      var avgRatings = gameRatings.Select(gr => gr.Rating).Average();
      var totalRatings = gameRatings.Count;

      var gameRatingListUsersResponse = new RatingListUsersResponse
      {
        UsersRatings = gameRatings
              .Select(gr => RatingMapper.ToGameUserResponse(gr))
              .ToList(),
        AvgRatings = Math.Round(Convert.ToDouble(avgRatings), 1),
        TotalRatings = totalRatings
      };


      return Response.Ok(gameRatingListUsersResponse);
    }
  }
}
