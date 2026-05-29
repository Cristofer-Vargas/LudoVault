using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Validations.Base;

namespace LudoVault.Services
{
  public class GameServices(IGameRepository gameRepo, IPlatformRepository platformRepo, IGenreRepository genreRepo,
    IPublisherRepository publisherRepo, IImageServices imageServices, ISystemServices sistema,
    ILogger<GameServices> logger) : IGameServices
  {
    private readonly IGameRepository _gameRepository = gameRepo;
    private readonly IPlatformRepository _platformRepository = platformRepo;
    private readonly IGenreRepository _genreRepository = genreRepo;
    private readonly IPublisherRepository _publisherRepository = publisherRepo;
    private readonly IImageServices _imageServices = imageServices;
    private readonly ISystemServices _sistema = sistema;

    private readonly ILogger<GameServices> _logger = logger;

    // Jogo
    public async Task<Response<GameResponse>> CriarGameAsync(GameRequest gameRequest)
    {
      var inputErrors = new List<Report>();
      var response = new Response<GameResponse>(inputErrors);

      if (string.IsNullOrWhiteSpace(gameRequest.Name))
        inputErrors.Add(Report.Create("Nome deve ser preenchido corretamente.", 400));
      if (string.IsNullOrWhiteSpace(gameRequest.Description))
        inputErrors.Add(Report.Create("Descrição deve ser preenchido corretamente.", 400));
      if (string.IsNullOrWhiteSpace(gameRequest.PublisherId.ToString()))
        inputErrors.Add(Report.Create("A Publisher deve ser informada corretamente.", 400));
      if (!gameRequest.GenreIds.Any() || !gameRequest.PlatformIds.Any())
        inputErrors.Add(Report.Create("Deve ser informado ao menos um gênero e plataforma corretamente.", 400));

      foreach (var platformId in gameRequest.PlatformIds)
      {
        var platform = await _platformRepository.BuscarPorId(platformId);
        if (platform == null)
        {
          inputErrors.Add(Report.Create($"Plataforma {platformId} não encontrada!", 404));
        }
      }
      foreach (var genreId in gameRequest.GenreIds)
      {
        var genre = await _genreRepository.BuscarPorId(genreId);
        if (genre == null)
        {
          inputErrors.Add(Report.Create($"Gênero {genreId} não encontrada!", 404));
        }
      }
      if (inputErrors.Count > 0)
        return response;

      var publisher = await _publisherRepository.BuscarPorIdAsync(gameRequest.PublisherId);
      if (publisher == null)
      {
        response.Report.Add(Report.Create("Não foi possivel encontrar a Publisher informada.", 404));
        return response;
      }

      gameRequest.ImageUrl = _sistema.CaminhoGameDefaultImage();
      var gameModel = GameMapper.ToModel(
          gameRequest,
          publisher,
          gameRequest.PlatformIds,
          gameRequest.GenreIds
          );

      var game = await _gameRepository.CriarAsync(gameModel);
      if (game == null)
      {
        _logger.LogError("Erro interno ao criar jogo {GNAME}.", gameRequest.Name);
        response.Report.Add(Report.Create("Erro ao criar jogo.", 500));
        return response;
      }

      _logger.LogInformation("Jogo {GID}:{GNAME} criado com sucesso.", game.Id, game.Name);
      response.Data = GameMapper.ToResponse(game);
      return response;
    }
    public async Task<Response<GameResponse>> AtualizarGameAsync(GameRequest gameRequest, int id)
    {
      var inputErrors = new List<Report>();
      var response = new Response<GameResponse>(inputErrors);

      var game = await _gameRepository.BuscarPorIdAsync(id);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      var publisher = await _publisherRepository.BuscarPorIdAsync(gameRequest.PublisherId);
      if (publisher == null)
      {
        response.Report.Add(Report.Create("Não foi possivel encontrar a Publisher informada."));
        return response;
      }

      if (string.IsNullOrWhiteSpace(game.ImageUrl))
      {
        gameRequest.ImageUrl = _sistema.CaminhoGameDefaultImage();
      }

      foreach (var platformId in gameRequest.PlatformIds)
      {
        var platform = await _platformRepository.BuscarPorId(platformId);
        if (platform == null)
        {
          inputErrors.Add(Report.Create($"Plataforma {platformId} não encontrada!", 404));
        }
      }
      foreach (var genreId in gameRequest.GenreIds)
      {
        var genre = await _genreRepository.BuscarPorId(genreId);
        if (genre == null)
        {
          inputErrors.Add(Report.Create($"Gênero {genreId} não encontrada!", 404));
        }
      }
      if (inputErrors.Count > 0)
      {
        return response;
      }

      game.Name = gameRequest.Name;
      game.Description = gameRequest.Description;
      game.PublisherId = gameRequest.PublisherId;
      game.Publisher = publisher;

      game.GamePlatforms = gameRequest.PlatformIds
        .Select(id => PlatformMapper.ToGamePlatformModel(id)) // Transforma em Platform Model pois Entidade Game só aceita lista do Model
        .ToList();
      game.GameGenres = gameRequest.GenreIds
        .Select(id => GenreMapper.ToGameGenreModel(id))
        .ToList();

      var newGame = await _gameRepository.AtualizarAsync(game);
      if (newGame == null)
      {
        _logger.LogWarning("Erro ao atualizar jogo {GID}:{GNAME}!", game.Id, game.Name);
        response.Report.Add(Report.Create("Erro interno ao atualizar jogo!", 500));
        return response;
      }

      _logger.LogInformation("Jogo {GID}:{GNAME} atualizado com sucesso.", newGame.Id, newGame.Name);
      response.Data = GameMapper.ToResponse(newGame);
      return response;
    }
    public async Task<Response<List<GameResponse>>> BuscarTodosGamesAsync()
    {
      var response = new Response<List<GameResponse>>();

      var gamesModel = await _gameRepository.BuscarTodosAsync();
      if (gamesModel == null)
      {
        _logger.LogWarning("Nenhum Jogo Encontrado!");
        response.Report.Add(Report.Create("Nenhum Jogo Encontrado!", 404));
        return response;
      }

      response.Data = gamesModel.Select(game => GameMapper.ToResponse(game)).ToList();
      return response;
    }
    public async Task<Response<GameResponse>> BuscarGamePorIdAsync(int id)
    {
      var response = new Response<GameResponse>();

      var gameModel = await _gameRepository.BuscarPorIdAsync(id);
      if (gameModel == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;

      }
      response.Data = GameMapper.ToResponse(gameModel);
      return response;
    }
    public async Task<Response<string>> AdicionarImagemDeCapaAsync(IFormFile image, int gameId)
    {
      var response = new Response<string>();

      var game = await _gameRepository.BuscarPorIdAsync(gameId);
      if (game == null)
      {
        _logger.LogWarning("Jogo com ID {GID} não encontrado para adicionar imagem.", gameId);
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      _logger.LogInformation("Adicionando Imagem para jogo {GID}:{GNAME}.", game.Id, game.Name);
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
      var imageUpdated = await _gameRepository.AtualizarCaminhoDeImagem(game);
      if (!imageUpdated)
      {
        response.Report.Add(Report.Create("Erro interno ao atualizar imagem!", 500));
        return response;
      }

      response.Data = caminhoImg;
      return response;
    }
    public async Task<Response<string>> RemoverGameAsync(int gameId)
    {
      var response = new Response<string>();

      var game = await _gameRepository.BuscarPorIdAsync(gameId);
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

      var deletado = await _gameRepository.ExcluirAsync(game);
      if (!deletado)
      {
        _logger.LogError("Erro ao excluir jogo com ID {GID}.", game.Id);
        response.Report.Add(Report.Create("Erro ao excluir jogo do banco de dados."));
        return response;
      }
      _logger.LogInformation("Jogo {GID}:{GNAME} excluído com sucesso.", game.Id, game.Name);
      response.Data = "Jogo excluído com sucesso!";
      return response;
    }
    public async Task<Response<string>> RemoverImagemDeCapaAsync(int gameId)
    {
      var response = new Response<string>();

      var game = await _gameRepository.BuscarPorIdAsync(gameId);
      if (game == null)
      {
        _logger.LogWarning("Jogo com ID {GID} não encontrado para remover imagem.", gameId);
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }
      _logger.LogInformation("Removendo Imagem de {GID}:{GNAME}.", game.Id, game.Name);

      var pathGameDefaultImage = _sistema.CaminhoGameDefaultImage();
      if (game.ImageUrl != pathGameDefaultImage)
      {
        if (!_imageServices.ExcluirImagemAsset(game.ImageUrl ?? ""))
        {
          response.Report.Add(Report.Create("Não foi possivel excluir essa imagem!"));
          return response;
        }

      }
      if (game.ImageUrl == pathGameDefaultImage)
      {
        _logger.LogInformation("Jogo {GID}:{GNAME} não possui imagem cadastrada.", game.Id, game.Name);
        response.Data = "Esse jogo não possui uma imagem.";
        return response;
      }

      game.ImageUrl = pathGameDefaultImage;
      var imagemUpdated = await _gameRepository.AtualizarCaminhoDeImagem(game);
      if (!imagemUpdated)
      {
        _logger.LogError("Erro interno ao excluir imagem {PATH}", game.ImageUrl);
        response.Report.Add(Report.Create("Erro interno ao remover imagem!", 500));
        return response;
      }

      response.Data = "Imagem removida com sucesso!";
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
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      var gameRatings = await _gameRepository.BuscarAvaliacoesAsync(id);
      if (gameRatings.Count == 0)
      {
        response.Report.Add(Report.Create("Nenhuma avaliação encontrada!", 404));
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
      return response;
    }
  }
}
