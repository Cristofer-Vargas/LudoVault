using LudoVault.Application.DTO.Requests;
using LudoVault.Application.DTO.Responses;
using LudoVault.Domain.Interfaces.Repositories;
using LudoVault.Application.Interfaces.Services;
using LudoVault.Application.Services.Mapper;
using LudoVault.Application.Validations;
using LudoVault.Application.Validations.Base;
using Microsoft.Extensions.Options;
using LudoVault.Application.Configurations;

namespace LudoVault.Application.Services
{
  public class GameServices(IGameRepository gameRepo, IPlatformRepository platformRepo, IGenreRepository genreRepo,
    IPublisherRepository publisherRepo, IImageServices imageServices, IOptions<DefaultImagesOptions> defaultImagesOptions,
    ILogger<GameServices> logger) : IGameServices
  {
    private readonly IGameRepository _gameRepository = gameRepo;
    private readonly IPlatformRepository _platformRepository = platformRepo;
    private readonly IGenreRepository _genreRepository = genreRepo;
    private readonly IPublisherRepository _publisherRepository = publisherRepo;
    private readonly IImageServices _imageServices = imageServices;
    private readonly DefaultImagesOptions _defaultImages = defaultImagesOptions.Value;

    private readonly ILogger<GameServices> _logger = logger;

    private async Task<List<Report>> ValidarPlataformasEGenerosAsync(GameRequest gameRequest, Response<GameResponse> response)
    {
      var reports = new List<Report>();
      foreach (var platformId in gameRequest.PlatformIds)
      {
        var platform = await _platformRepository.BuscarPorId(platformId);
        if (platform == null)
        {
          response.Report.Add(Report.Create($"Plataforma com ID {platformId} não encontrada!", 404));
        }
      }
      foreach (var genreId in gameRequest.GenreIds)
      {
        var genre = await _genreRepository.BuscarPorId(genreId);
        if (genre == null)
        {
          response.Report.Add(Report.Create($"Gênero com ID {genreId} não encontrado!", 404));
        }
      }
      return reports;
    }

    // Jogo
    public async Task<Response<GameResponse>> CriarGameAsync(GameRequest gameRequest)
    {
      var response = new Response<GameResponse>();

      var validation = new GameValidation();
      var errors = validation.Validate(gameRequest).GetErrors();

      if (!errors.IsSuccessul)
        return new Response<GameResponse>(errors.Report);

      gameRequest.PlatformIds = gameRequest.PlatformIds.Distinct().ToList();
      gameRequest.GenreIds = gameRequest.GenreIds.Distinct().ToList();

      var platformAndGenreErrors = await ValidarPlataformasEGenerosAsync(gameRequest, response);
      if (platformAndGenreErrors.Count > 0)
        return new Response<GameResponse>(platformAndGenreErrors);

      var publisher = await _publisherRepository.BuscarPorIdAsync(gameRequest.PublisherId);
      if (publisher == null)
      {
        response.Report.Add(Report.Create("Não foi possivel encontrar a Publisher informada.", 404));
      }

      if (!response.IsSuccessul)
        return response;

      gameRequest.ImageUrl = _defaultImages.GameImage;
      var gameModel = GameMapper.ToModel(
          gameRequest,
          publisher!,
          gameRequest.PlatformIds,
          gameRequest.GenreIds
          );

      var game = await _gameRepository.CriarAsync(gameModel);
      if (game == null)
      {
        _logger.LogError("Erro interno ao criar jogo {GNAME}.", gameRequest.Name);
        response.Report.Add(Report.Create($"Erro interno ao criar jogo {gameRequest.Name}.", 500));
        return response;
      }

      _logger.LogInformation("Jogo {GID}:{GNAME} criado com sucesso.", game.Id, game.Name);
      response.Data = GameMapper.ToResponse(game);
      response.Status = 201;
      return response;
    }
    public async Task<Response<GameResponse>> AtualizarGameAsync(GameRequest gameRequest, int id)
    {
      var response = new Response<GameResponse>();

      var validation = new GameValidation();
      var errors = validation.Validate(gameRequest).GetErrors();

      if (!errors.IsSuccessul)
        return new Response<GameResponse>(errors.Report);

      gameRequest.PlatformIds = gameRequest.PlatformIds.Distinct().ToList();
      gameRequest.GenreIds = gameRequest.GenreIds.Distinct().ToList();

      var platformAndGenreErrors = await ValidarPlataformasEGenerosAsync(gameRequest, response);
      if (platformAndGenreErrors.Count > 0)
        return new Response<GameResponse>(platformAndGenreErrors);

      var game = await _gameRepository.BuscarPorIdAsync(id);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
      }

      var publisher = await _publisherRepository.BuscarPorIdAsync(gameRequest.PublisherId);
      if (publisher == null)
      {
        response.Report.Add(Report.Create("Não foi possivel encontrar a Publisher informada.", 404));
      }

      if (!response.IsSuccessul)
      {
        return response;
      }

      game!.Name = gameRequest.Name;
      game.Description = gameRequest.Description;
      game.PublisherId = gameRequest.PublisherId;
      game.Publisher = publisher!;

      game.GamePlatforms = gameRequest.PlatformIds
        .Select(id => PlatformMapper.ToGamePlatformModel(id))
        .ToList();
      game.GameGenres = gameRequest.GenreIds
        .Select(id => GenreMapper.ToGameGenreModel(id))
        .ToList();

      var newGame = await _gameRepository.AtualizarAsync(game);
      if (newGame == null)
      {
        _logger.LogWarning("Erro ao atualizar jogo {GID}:{GNAME}!", game.Id, game.Name);
        response.Report.Add(Report.Create($"Erro interno ao atualizar jogo {game.Name}!", 500));
        return response;
      }

      _logger.LogInformation("Jogo {GID}:{GNAME} atualizado com sucesso.", newGame.Id, newGame.Name);
      response.Data = GameMapper.ToResponse(newGame);
      response.Status = 200;
      return response;
    }
    public async Task<Response<List<GameResponse>>> BuscarTodosGamesAsync()
    {
      var response = new Response<List<GameResponse>>();

      var gamesModel = await _gameRepository.BuscarTodosAsync();
      if (gamesModel == null || gamesModel.Count == 0)
      {
        _logger.LogWarning("Erro interno ou nenhum jogo cadastrado!");
        response.Report.Add(Report.Create("Erro interno ou nenhum jogo cadastrado!", 404));
        return response;
      }

      response.Data = gamesModel.Select(game => GameMapper.ToResponse(game)).ToList();
      response.Status = 200;
      return response;
    }
    public async Task<Response<GameResponse>> BuscarGamePorIdAsync(int id)
    {
      var response = new Response<GameResponse>();

      var gameModel = await _gameRepository.BuscarPorIdAsync(id);
      if (gameModel == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
        return response;

      }
      response.Data = GameMapper.ToResponse(gameModel);
      response.Status = 200;
      return response;
    }
    public async Task<Response<GameResponse>> AdicionarImagemDeCapaAsync(IFormFile image, int gameId)
    {
      var response = new Response<GameResponse>();

      var game = await _gameRepository.BuscarPorIdAsync(gameId);
      if (game == null)
      {
        _logger.LogWarning("Jogo com ID {GID} não encontrado para adicionar imagem.", gameId);
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
        return response;
      }

      var oldImageUrl = game.ImageUrl;
      var defaultImageUrl = _defaultImages.GameImage;

      _logger.LogInformation("Adicionando Imagem para jogo {GID}:{GNAME}.", game.Id, game.Name);

      var caminhoImg = await _imageServices.ConverteParaWebpESalvaImagem(image, "games");

      if (caminhoImg == defaultImageUrl && (image == null || image.Length == 0))
      {
        response.Report.Add(Report.Create("Nenhuma imagem enviada!", 400));
        return response;
      }

      game.ImageUrl = caminhoImg;
      var imageUpdated = await _gameRepository.AtualizarCaminhoDeImagem(game);

      if (!imageUpdated)
      {
        _logger.LogError("Erro interno ao atualizar imagem de {GID}:{GNAME}.", game.Id, game.Name);
        if (caminhoImg != defaultImageUrl)
        {
          _imageServices.ExcluirImagemAsset(caminhoImg);
        }
        response.Report.Add(Report.Create("Erro interno ao atualizar imagem!", 500));
        return response;
      }

      if (oldImageUrl != defaultImageUrl && !string.IsNullOrEmpty(oldImageUrl))
      {
        _logger.LogInformation("Substituindo Imagem existente...");
        _imageServices.ExcluirImagemAsset(oldImageUrl);
      }

      response.Data = GameMapper.ToResponse(game);
      response.Status = 201;
      return response;
    }
    public async Task<Response<string>> RemoverGameAsync(int gameId)
    {
      var response = new Response<string>();

      var game = await _gameRepository.BuscarPorIdAsync(gameId);
      if (game == null)
      {
        _logger.LogWarning("Tentativa de exclusão de jogo inexistente. ID: {GID}", gameId);
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
        return response;
      }

      if (game.ImageUrl != _defaultImages.GameImage)    // Garantir que se for imagem dafult, não o exclua do servidor
      {
        if (!_imageServices.ExcluirImagemAsset(game.ImageUrl))
        {
          response.Report.Add(Report.Create("Erro ao excluir imagem do servidor!", 500));
          return response;
        }
      }

      var deletado = await _gameRepository.ExcluirAsync(game);
      if (!deletado)
      {
        _logger.LogError("Erro ao excluir jogo {GID}:{GNAME}.", game.Id, game.Name);
        response.Report.Add(Report.Create("Erro interno ao excluir jogo.", 500));
        return response;
      }
      _logger.LogInformation("Jogo {GID}:{GNAME} excluído com sucesso.", game.Id, game.Name);
      response.Data = $"Jogo {game.Name} excluído com sucesso!";
      response.Status = 200;
      return response;
    }
    public async Task<Response<GameResponse>> RemoverImagemDeCapaAsync(int gameId)
    {
      var response = new Response<GameResponse>();

      var game = await _gameRepository.BuscarPorIdAsync(gameId);
      if (game == null)
      {
        _logger.LogWarning("Jogo com ID {GID} não encontrado para remover imagem.", gameId);
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
        return response;
      }
      _logger.LogInformation("Removendo Imagem de {GID}:{GNAME}.", game.Id, game.Name);

      var pathGameDefaultImage = _defaultImages.GameImage;
      if (game.ImageUrl != pathGameDefaultImage)
      {
        if (!_imageServices.ExcluirImagemAsset(game.ImageUrl ?? ""))
        {
          response.Report.Add(Report.Create("Não foi possivel excluir essa imagem!", 500));
          return response;
        }

      }
      if (game.ImageUrl == pathGameDefaultImage)
      {
        _logger.LogInformation("Jogo {GID}:{GNAME} não possui imagem cadastrada.", game.Id, game.Name);
        response.Report.Add(Report.Create($"Jogo {game.Name} não possui uma imagem.", 400));
        return response;
      }

      game.ImageUrl = pathGameDefaultImage;
      var imagemUpdated = await _gameRepository.AtualizarCaminhoDeImagem(game);
      if (!imagemUpdated)
      {
        _logger.LogError("Erro interno ao remover imagem de {GID}:{GNAME}", game.Id, game.Name);
        response.Report.Add(Report.Create("Erro interno ao remover imagem!", 500));
        return response;
      }

      response.Data = GameMapper.ToResponse(game);
      response.Status = 200;
      return response;
    }

    // Avaliações de Jogo
    public async Task<Response<RatingListUsersResponse>> BuscarAvaliacoesPorJogoAsync(int id)
    {
      var response = new Response<RatingListUsersResponse>();
      var game = await _gameRepository.BuscarPorIdAsync(id);
      if (game == null)
      {
        _logger.LogWarning("Jogo com ID {GID} não encontrado para busca de avaliações.", id);
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
        return response;
      }

      var gameRatings = await _gameRepository.BuscarAvaliacoesAsync(id);
      if (gameRatings.Count == 0)
      {
        response.Report.Add(Report.Create($"Nenhuma avaliação de {game.Name} encontrada!", 404));
        return response;
      }

      var avgRatings = gameRatings.Select(gr => gr.Rating).Average();
      var totalRatings = gameRatings.Count;

      response.Data = new RatingListUsersResponse
      {
        UsersRatings = gameRatings
              .Select(gr => RatingMapper.ToGameUserResponse(gr))
              .ToList(),
        AvgRatings = Math.Round(Convert.ToDouble(avgRatings), 1),
        TotalRatings = totalRatings
      };
      response.Status = 200;
      return response;
    }
  }
}
