using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Validations.Base;

namespace LudoVault.Services
{
  public class GameServices(IGameRepository gameRepo, IPublisherRepository publisherRepo,
    IImageServices imageServices, ISystemServices sistema, ILogger<GameServices> logger) : IGameServices
  {
    private readonly IGameRepository _gameRepository = gameRepo;
    private readonly IPublisherRepository _publisherRepository = publisherRepo;
    private readonly IImageServices _imageServices = imageServices;
    private readonly ISystemServices _sistema = sistema;

    private readonly ILogger<GameServices> _logger = logger;

    // Jogo
    public async Task<Response<List<GameResponse>>> BuscarTodosGamesAsync()
    {
      var response = new Response<List<GameResponse>>();

      var gamesModel = await _gameRepository.BuscarTodosGamesAsync();
      if (gamesModel == null)
      {
        _logger.LogWarning("Nenhum Jogo Encontrado!");
        return Response.Ok(new List<GameResponse>());
      }

      var games = gamesModel.Select(game => GameMapper.ToResponse(game)).ToList();
      return Response.Ok(games);
    }
    public async Task<Response<GameResponse>> BuscarGamePorIdAsync(int id)
    {
      var response = new Response<GameResponse>();

      var gameModel = await _gameRepository.BuscarGamePorIdAsync(id);
      if (gameModel == null)
      {
        _logger.LogWarning("Jogo com ID {GID} não encontrado!", id);
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
        _logger.LogWarning("Não foi possível encontrar a Publisher com ID {PID} para o novo jogo.", request.PublisherId);
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
      _logger.LogInformation("Jogo {GNAME} com ID {GID} criado.", game.Name, game.Id);
      return Response.Ok(GameMapper.ToResponse(game));
    }
    public async Task<Response<GameResponse>> AtualizarGameAsync(GameRequest game, int id)
    {
      var response = new Response<GameResponse>();

      var currentGame = await _gameRepository.BuscarGamePorIdAsync(id);
      if (currentGame == null)
      {
        _logger.LogWarning("Jogo com ID {GID} não encontrado para atualização.", id);
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      var publisher = await _publisherRepository.BuscarPublisherPorIdAsync(game.PublisherId);
      if (publisher == null)
      {
        _logger.LogWarning("Não foi possível encontrar a Publisher com ID {PID} para atualização do jogo.", game.PublisherId);
        response.Report.Add(Report.Create("Não foi possivel encontrar a Publisher responsável."));
        return response;
      }

      if (string.IsNullOrWhiteSpace(currentGame.ImageUrl))
      {
        _logger.LogInformation("Imagem do jogo nunca cadastrada anteriormente. Usando imagem padrão.");
        game.ImageUrl = _sistema.CaminhoGameDefaultImage();
      }

      currentGame.Name = game.Name;
      currentGame.Description = game.Description;
      currentGame.PublisherId = game.PublisherId;
      currentGame.Publisher = publisher;

      currentGame.GamePlatforms = game.PlatformIds.Select(id => PlatformMapper.ToGamePlatformModel(id)).ToList();
      currentGame.GameGenres = game.GenreIds.Select(id => GenreMapper.ToGameGenreModel(id)).ToList();

      var newGame = await _gameRepository.AtualizarGameAsync(currentGame);
      if (newGame == null)
      {
        _logger.LogWarning("Erro ao processar atualização do jogo ID {GID}.", id);
        response.Report.Add(Report.Create("Não foi possivel encontrar jogo recem criado!"));
        return response;
      }

      _logger.LogInformation("Jogo {GNAME} com ID {GID} atualizado.", newGame.Name, newGame.Id);
      return Response.Ok(GameMapper.ToResponse(newGame));
    }
    public async Task<Response<string>> RemoverGameAsync(int gameId)
    {
      var response = new Response<string>();

      var game = await _gameRepository.BuscarGamePorIdAsync(gameId);

      if (game == null)
      {
        _logger.LogWarning("Tentativa de exclusão de jogo inexistente. ID: {GID}", gameId);
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

      var deletado = await _gameRepository.DeletarGameAsync(game);
      if (!deletado)
      {
        _logger.LogError("Erro ao excluir jogo com ID {GID}.", game.Id);
        response.Report.Add(Report.Create("Erro ao excluir jogo do banco de dados."));
        return response;
      }
      _logger.LogInformation("Jogo {GNAME} com ID {GID} excluído!", game.Name, game.Id);
      return Response.Ok("Jogo excluído com sucesso!");
    }
    public async Task<Response<string>> AdicionarImagemDeCapaAsync(IFormFile image, int gameId)
    {
      var response = new Response<string>();

      var game = await _gameRepository.BuscarGamePorIdAsync(gameId);

      if (game == null)
      {
        _logger.LogWarning("Jogo com ID {GID} não encontrado para adicionar imagem.", gameId);
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      _logger.LogInformation("Adicionando Imagem para jogo {GNAME} com ID {GID}", game.Name, game.Id);
      if (game.ImageUrl != _sistema.CaminhoGameDefaultImage())
      {
        _logger.LogInformation("Substituindo Imagem existente.");
        if (!_imageServices.ExcluirImagemAsset(game.ImageUrl))
        {
          response.Report.Add(Report.Create("Erro ao excluir imagem do servidor!"));
          return response;
        }
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
        _logger.LogWarning("Jogo com ID {GID} não encontrado para remover imagem.", gameId);
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }
      _logger.LogInformation("Removendo Imagem de {GNAME} com ID {GID}", game.Name, game.Id);

      var pathGameDefaultImage = _sistema.CaminhoGameDefaultImage();
      if (game.ImageUrl != pathGameDefaultImage)
      {
        if (!_imageServices.ExcluirImagemAsset(game.ImageUrl ?? ""))
        {
          response.Report.Add(Report.Create("Não foi possivel excluir essa imagem!"));
          return response;
        }

      }
      else if (game.ImageUrl == pathGameDefaultImage)
      {
        _logger.LogInformation("Jogo {GNAME} com ID {GID} não possui imagem cadastrada.", game.Name, game.Id);
        response.Data = "Esse jogo não possui uma imagem.";
        return response;
      }

      game.ImageUrl = pathGameDefaultImage;
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
        _logger.LogWarning("Jogo com ID {GID} não encontrado para busca de avaliações.", id);
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
