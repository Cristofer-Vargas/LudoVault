using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Validations.Base;

namespace LudoVault.Services
{
  public class UserServices(IUserRepository userRepo, ISecurityServices securityService,
    IGameRepository gameRepo, IImageServices imageServices, ISystemServices sistema, ILogger<UserServices> logger) : IUserServices
  {
    private readonly IUserRepository _userRepository = userRepo;
    private readonly IGameRepository _gameRepository = gameRepo;
    private readonly ISecurityServices _securityService = securityService;
    private readonly IImageServices _imageServices = imageServices;
    private readonly ISystemServices _sistema = sistema;
    private readonly ILogger<UserServices> _logger = logger;

    // Usuário
    public async Task<Response<UserResponse>> CriarUsuarioAsync(UserRequest userRequest)
    {
      var inputErrors = new List<Report>();
      var response = new Response<UserResponse>(inputErrors);

      if (string.IsNullOrWhiteSpace(userRequest.Email))
        inputErrors.Add(Report.Create("Email deve ser preenchido corretamente!", 400));
      if (string.IsNullOrWhiteSpace(userRequest.Name))
        inputErrors.Add(Report.Create("Nome deve ser definido corretamente!", 400));
      if (string.IsNullOrWhiteSpace(userRequest.PasswordHash))
        inputErrors.Add(Report.Create("Senha obrigatória deve ser preenchida corretamente!", 400));
      if (inputErrors.Count > 0)
        return response;

      var emailExist = await _userRepository.VerificarEmailExistenteAsync(null, userRequest.Email.Trim());
      if (emailExist)
      {
        response.Report.Add(Report.Create($"Email {userRequest.Email} ja em uso!", 400));
        return response;
      }

      userRequest.AvatarUrl = _sistema.CaminhoUserDefaultImage();
      userRequest.PasswordHash = await _securityService.EncryptPassword(userRequest.PasswordHash ?? "");
      var userModel = UserMapper.ToModel(userRequest, userRequest.PasswordHash);

      var currentUser = await _userRepository.CriarUsuarioAsync(userModel);
      if (currentUser == null)
      {
        _logger.LogError("Erro ao criar usuário {UNAME}.", userRequest.Name);
        response.Report.Add(Report.Create($"Erro interno ao criar usuário {userRequest.Name}.", 500));
        return response;
      }

      _logger.LogInformation("Usuário {UID}:{UNAME} criado com sucesso.", currentUser.Id, currentUser.Name);
      response.Data = UserMapper.ToResponse(currentUser);
      return response;
    }
    public async Task<Response<UserResponse>> AtualizarUsuarioAsync(UserRequest userRequest, int userID)
    {
      var inputErrors = new List<Report>();
      var response = new Response<UserResponse>(inputErrors);

      if (string.IsNullOrWhiteSpace(userRequest.Email))
        inputErrors.Add(Report.Create("Email deve ser preenchido corretamente!", 400));
      if (string.IsNullOrWhiteSpace(userRequest.Name))
        inputErrors.Add(Report.Create("Nome deve ser definido corretamente!", 400));
      if (inputErrors.Count > 0)
        return response;

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userID);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      if (!string.IsNullOrWhiteSpace(userRequest.PasswordHash))
        userRequest.PasswordHash = user.PasswordHash;

      var emailExist = await _userRepository.VerificarEmailExistenteAsync(userID, userRequest.Email.Trim());
      if (emailExist)
      {
        response.Report.Add(Report.Create($"Email {userRequest.Email} em uso!", 400));
        return response;
      }

      user.Name = userRequest.Name ?? "";
      user.Email = userRequest.Email ?? "";
      user.PasswordHash = await _securityService.EncryptPassword(userRequest.PasswordHash ?? "");
      user.Bio = userRequest.Bio;

      var currentUser = await _userRepository.AtualizarUsuarioAsync(user);
      if (currentUser == null)
      {
        _logger.LogError("Erro ao atualizar usuário {UID}:{UNAME}", user.Id, user.Name);
        response.Report.Add(Report.Create($"Erro interno ao atualizar usuário {user.Name}.", 500));
        return response;
      }

      _logger.LogInformation("Usuário {UID}:{UNAME} atualizado.", currentUser.Id, currentUser.Name);
      response.Data = UserMapper.ToResponse(currentUser);
      return response;
    }
    public async Task<Response<UserResponse>> BuscarUsuarioPorIdAsync(int id)
    {
      var response = new Response<UserResponse>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(id);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      response.Data = UserMapper.ToResponse(user);
      return response;
    }
    public async Task<Response<UserResponse>> AdicionarImagemDePerfilAsync(IFormFile image, int userId)
    {
      var response = new Response<UserResponse>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      if (user.AvatarUrl != _sistema.CaminhoUserDefaultImage())
      {
        var imageDeleted = _imageServices.ExcluirImagemAsset(user.AvatarUrl);
        if (!imageDeleted && !string.IsNullOrWhiteSpace(user.AvatarUrl))
        {
          response.Report.Add(Report.Create("Erro interno ao substituir imagem.", 500));
          return response;
        }
        user.AvatarUrl = _sistema.CaminhoUserDefaultImage();
      }

      var caminho = await _imageServices.ConverteParaWebpESalvaImagem(image, "users");
      user.AvatarUrl = caminho;
      var imgUpdated = await _userRepository.AtualizarImagemDePerfilAsync(user);
      if (!imgUpdated)
      {
        _logger.LogError("Erro ao atualizar imagem {UIMG} de usuário {UID}:{UNAME}", caminho, user.Id, user.Name);
        response.Report.Add(Report.Create("Erro interno ao atualizar imagem.", 500));
        return response;
      }

      response.Data = UserMapper.ToResponse(user);
      _logger.LogInformation("Imagem de {UID}:{UNAME} atualizada com sucesso.", user.Id, user.Name);
      return response;
    }
    public async Task<Response<UserResponse>> RemoverImagemDePerfilAsync(int userId)
    {
      var response = new Response<UserResponse>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      _logger.LogInformation("Removendo imagem de {UID}:{UNAME}.", user.Id, user.Name);
      if (user.AvatarUrl != _sistema.CaminhoUserDefaultImage())
      {
        var imageDeleted = _imageServices.ExcluirImagemAsset(user.AvatarUrl ?? "");
        if (!imageDeleted)
        {
          response.Report.Add(Report.Create("Erro interno ao excluir imagem!", 500));
          return response;
        }
      }

      user.AvatarUrl = _sistema.CaminhoUserDefaultImage();
      var imgUpdated = await _userRepository.AtualizarImagemDePerfilAsync(user);
      if (!imgUpdated)
      {
        _logger.LogError("Erro ao remover imagem de {UID}:{UNAME}.", user.Id, user.Name);
        response.Report.Add(Report.Create("Erro interno ao atualizar imagem.", 500));
        return response;
      }

      response.Data = UserMapper.ToResponse(user);
      return response;
    }
    public async Task<Response<string>> ExcluirUsuarioAsync(int userId)
    {
      var response = new Response<string>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      if (user.AvatarUrl != _sistema.CaminhoUserDefaultImage() && !string.IsNullOrWhiteSpace(user.AvatarUrl))
      {
        var imageDeleted = _imageServices.ExcluirImagemAsset(user.AvatarUrl);
        if (!imageDeleted)
        {
          response.Report.Add(Report.Create("Erro interno ao excluir imagem.", 500));
          return response;
        }
      }
      user.AvatarUrl = _sistema.CaminhoUserDefaultImage();

      var userExcluded = await _userRepository.ExcluirUsuarioAsync(user);
      if (!userExcluded)
      {
        _logger.LogError("Erro ao excluir usuario {UID}:{UNAME}!", user.Id, user.Name);
        response.Report.Add(Report.Create("Erro interno ao excluir usuário.", 500));
        return response;
      }

      response.Data = $"Usuário {user.Name} excluido com sucesso!";
      _logger.LogInformation("Usuario {UID}:{UNAME} excluido com sucesso!", user.Id, user.Name);
      return response;
    }

    // Listas de Usuário
    public async Task<Response<UserListListsResponse>> CriarListaAsync(UserListRequest userList)
    {
      var response = new Response<UserListListsResponse>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userList.UserId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      var existeListaComMesmoNome = await _userRepository.ExisteListaComMesmoNomeAsync(userList.Name ?? string.Empty, userList.UserId);
      if (existeListaComMesmoNome)
      {
        response.Report.Add(Report.Create($"Existe outra lista com nome {userList.Name}.", 400));
        return response;
      }

      var userListModel = UserListMapper.ToUserListModel(userList, userList.UserId);
      var lista = await _userRepository.CriarListaAsync(userListModel);
      if (lista == null)
      {
        response.Report.Add(Report.Create("Erro interno ao retornar lista recem criada!", 500));
        return response;
      }

      response.Data = UserListMapper.ToListGameResponse(lista);
      _logger.LogInformation("Lista {LID}:{LNAME} criada por {UID}:{UNAME}.", lista.Id, lista.Name, user.Id, user.Name);
      return response;
    }
    public async Task<Response<UserListListsResponse>> AtualizarListaAsync(UserListRequest userList, int listId)
    {
      var response = new Response<UserListListsResponse>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userList.UserId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      var list = await _userRepository.BuscarListaAsync(listId);
      if (list == null)
      {
        response.Report.Add(Report.Create("Lista não encontrada!", 404));
        return response;
      }

      var existeListaComMesmoNome = await _userRepository.ExisteListaComMesmoNomeAsync(userList.Name ?? string.Empty, userList.UserId);
      if (existeListaComMesmoNome)
      {
        response.Report.Add(Report.Create($"Existe outra lista com nome {userList.Name}.", 400));
        return response;
      }

      list.Name = userList.Name;
      var createdListModel = await _userRepository.AtualizarListaAsync(list);
      if (createdListModel == null)
      {
        response.Report.Add(Report.Create("Erro interno ao atualizar lista!", 500));
        return response;
      }

      response.Data = UserListMapper.ToListGameResponse(createdListModel);
      _logger.LogInformation("Lista {LID}:{LNAME} atualizada por {UID}:{UNAME} com sucesso.", createdListModel.Id, createdListModel.Name, user.Id, user.Name);
      return response;
    }
    public async Task<Response<UserListListsResponse>> AdicionarJogoAListaAsync(UserListGameRequest userGameList, int userId)
    {
      var response = new Response<UserListListsResponse>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      var list = await _userRepository.BuscarListaAsync(userGameList.ListId);
      if (list == null)
      {
        response.Report.Add(Report.Create("Lista não encontrada!", 404));
        return response;
      }

      var game = await _gameRepository.BuscarPorIdAsync(userGameList.GameId);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
        return response;
      }

      var gameExistInList = await _userRepository.BuscarJogoDaListaAsync(userGameList.GameId, userGameList.ListId);
      if (gameExistInList != null)
      {
        response.Report.Add(Report.Create("Esse jogo ja foi adicionado!", 400));
        return response;
      }

      var userListGameModel = UserListMapper.ToUserListGameModel(userGameList);
      var userListGame = await _userRepository.AdicionarJogoAListaAsync(userListGameModel);
      if (userListGame == null)
      {
        _logger.LogError("Erro ao retornar lista {LID}:{LNAME} para o usuário {UID}:{UNAME} após adicionar jogo {GID}:{GNAME}.", list.Id, list.Name, user.Id, user.Name, game.Id, game.Name);
        response.Report.Add(Report.Create("Erro interno ao retornar lista recém criada.", 500));
        return response;
      }

      response.Data = UserListMapper.ToListGameResponse(userListGame);
      _logger.LogInformation("Jogo {GID}:{GNAME} adicionado em {LID}:{LNAME} de {UID}:{UNAME}.", game.Id, game.Name, list.Id, list.Name, user.Id, user.Name);
      return response;
    }
    public async Task<Response<UserListResponse>> BuscarListasDeUsuarioAsync(int id)
    {
      var response = new Response<UserListResponse>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(id);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      var userLists = await _userRepository.BuscarListasDeUsuarioAsync(id);
      var totalLists = userLists.Count;
      response.Data = new UserListResponse
      {
        Lists = userLists
                      .Select(ul => UserListMapper.ToListGameResponse(ul)).ToList(),
        TotalLists = totalLists
      };
      return response;
    }
    public async Task<Response<string>> ExcluirListaAsync(int userId, int listId)
    {
      var response = new Response<string>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      var list = await _userRepository.BuscarListaAsync(listId);
      if (list == null)
      {
        response.Report.Add(Report.Create("Lista não econtrada.", 404));
        return response;
      }

      var listExcluded = await _userRepository.ExcluirListaAsync(list);
      if (!listExcluded)
      {
        _logger.LogError("Erro ao excluir a lista {LID}:{LNAME} de {UID}:{UNAME}", list.Id, list.Name, user.Id, user.Name);
        response.Report.Add(Report.Create($"Erro interno ao excluir a lista {list.Name}!", 500));
        return response;
      }

      response.Data = $"Lista {list.Name} excluída com sucesso!";
      _logger.LogInformation("Lista {LID}:{LNAME} excluida por {UID}:{UNAME}.", list.Id, list.Name, user.Id, user.Name);
      return response;
    }
    public async Task<Response<UserListListsResponse>> RemoverJogoDeListaAsync(int userId, int listId, int gameId)
    {
      var response = new Response<UserListListsResponse>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      var game = await _gameRepository.BuscarPorIdAsync(gameId);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
        return response;
      }

      var list = await _userRepository.BuscarListaAsync(listId);
      if (list == null)
      {
        response.Report.Add(Report.Create("Lista não econtrada.", 404));
        return response;
      }

      var gameExistInUserList = await _userRepository.BuscarJogoDaListaAsync(gameId, listId);
      if (gameExistInUserList == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado nessa lista!", 400));
        return response;
      }

      var listGameExcluded = await _userRepository.RemoverJogoDaListaAsync(gameExistInUserList);
      if (!listGameExcluded)
      {
        _logger.LogError("Erro ao remover jogo {GID}:{GNAME} da lista {LID}:{LNAME} do usuário {UID}:{UNAME}.", game.Id, game.Name, list.Id, list.Name, user.Id, user.Name);
        response.Report.Add(Report.Create("Erro interno ao remover jogo da lista!", 500));
        return response;
      }

      var updatedList = await _userRepository.BuscarListaAsync(listId);
      if (updatedList == null)
      {
        response.Report.Add(Report.Create("Erro ao recuperar lista atualizada.", 500));
        return response;
      }

      response.Data = UserListMapper.ToListGameResponse(updatedList);
      _logger.LogInformation("Jogo {GID}:{GNAME} removido da lista {LID}:{LNAME} por {UID}:{UNAME}", game.Id, game.Name, list.Id, list.Name, user.Id, user.Name);
      return response;
    }
    // Biblioteca de Usuário
    public async Task<Response<List<UserLibraryGameResponse>>> AdicionarJogoABibliotecaAsync(UserLibraryRequest userLibrary)
    {
      var response = new Response<List<UserLibraryGameResponse>>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userLibrary.UserId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado.", 404));
        return response;
      }

      var game = await _gameRepository.BuscarPorIdAsync(userLibrary.GameId);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
        return response;
      }

      var libraryGame = await _userRepository.BuscarPorIdJogoDaBibliotecaAsync(userLibrary.UserId, userLibrary.GameId);
      if (libraryGame != null)
      {
        response.Report.Add(Report.Create($"Jogo {game.Name} ja adicionado a biblioteca.", 400));
        return response;
      }

      var userLibraryModel = UserLibraryMapper.ToModel(userLibrary);
      var IsAdded = await _userRepository.AdicionarJogoNaBibliotecaAsync(userLibraryModel);
      if (!IsAdded)
      {
        _logger.LogError("Erro ao adicionar jogo {GID}:{GNAME} à biblioteca de {UID}:{UNAME}.", game.Id, game.Name, user.Id, user.Name);
        response.Report.Add(Report.Create($"Erro interno ao adicionar {game.Name} a biblioteca.", 500));
        return response;
      }

      _logger.LogInformation("Jogo {GID}:{GNAME} adicionado a biblioteca de {UID}:{UNAME}", game.Id, game.Name, user.Id, user.Name);
      
      var updatedLibrary = await _userRepository.BuscarJogosDaBibliotecaAsync(userLibrary.UserId);
      response.Data = updatedLibrary.Select(UserLibraryMapper.ToGameResponse).ToList();
      return response;
    }
    public async Task<Response<List<UserLibraryGameResponse>>> BuscarJogosDaBibliotecaAsync(int id)
    {
      var response = new Response<List<UserLibraryGameResponse>>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(id);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      var libraryGames = await _userRepository.BuscarJogosDaBibliotecaAsync(id);

      response.Data = libraryGames
              .Select(lg => UserLibraryMapper.ToGameResponse(lg))
              .ToList();

      return response;
    }
    public async Task<Response<List<UserLibraryGameResponse>>> RemoverJogoDaBibliotecaAsync(int userId, int gameId)
    {
      var response = new Response<List<UserLibraryGameResponse>>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      var game = await _gameRepository.BuscarPorIdAsync(gameId);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
        return response;
      }

      var libraryGame = await _userRepository.BuscarPorIdJogoDaBibliotecaAsync(userId, gameId);
      if (libraryGame == null)
      {
        response.Report.Add(Report.Create($"Jogo {game.Name} não encontrado na bilbioteca.", 404));
        return response;
      }

      var removido = await _userRepository.RemoverJogoDaBibliotecaAsync(libraryGame);
      if (!removido)
      {
        _logger.LogError("Erro ao remover jogo {GID}:{GNAME} da biblioteca de {UID}:{UNAME}.", game.Id, game.Name, user.Id, user.Name);
        response.Report.Add(Report.Create($"Erro interno ao remover jogo {game.Name} da biblioteca.", 500));
        return response;
      }

      _logger.LogInformation("Jogo {GID}:{GNAME} removido da biblioteca de {UID}:{UNAME}", game.Id, game.Name, user.Id, user.Name);
      var updatedLibrary = await _userRepository.BuscarJogosDaBibliotecaAsync(userId);
      response.Data = updatedLibrary.Select(UserLibraryMapper.ToGameResponse).ToList();
      return response;
    }

    // Avaliações de Usuário
    public async Task<Response<UserRatingListGamesResponse>> AdicionarAvaliacaoAsync(UserRatingRequest userRating, int userId, int gameId)
    {
      var inputErrors = new List<Report>();
      var response = new Response<UserRatingListGamesResponse>(inputErrors);

      if (string.IsNullOrWhiteSpace(userId.ToString()))
        inputErrors.Add(Report.Create("Obrigatório fornecer usuário para avaliação.", 400));
      if (string.IsNullOrWhiteSpace(userRating.Rating.ToString()))
        inputErrors.Add(Report.Create("Obrigatório fornecer uma avaliação.", 400));
      if (userRating.Rating < 0 || userRating.Rating > 5)
        inputErrors.Add(Report.Create("Avaliação deve ser um número válido de 0 a 5.", 400));
      if (inputErrors.Count > 0)
        return response;

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      var game = await _gameRepository.BuscarPorIdAsync(gameId);

      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
        return response;

      }
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      var rating = await _userRepository.BuscarAvaliacaoPorUserEGameAsync(userId, gameId);
      if (rating != null)
      {
        response.Report.Add(Report.Create($"Jogo {game.Name} ja avaliado!", 400));
        return response;
      }

      var userRatingModel = RatingMapper.ToRatingModel(userRating, user, game);
      var userRatings = await _userRepository.AdicionarAvaliacaoAsync(userRatingModel);
      var totalRatings = userRatings.Count;

      response.Data = new UserRatingListGamesResponse
      {
        Ratings = userRatings
              .Select(ur => RatingMapper.ToUserGameResponse(ur)).ToList(),
        TotalRatings = totalRatings
      };

      _logger.LogInformation("Avaliação de {GID}:{GNAME} adicionada por {UID}:{UNAME}", game.Id, game.Name, user.Id, user.Name);
      return response;
    }
    public async Task<Response<UserRatingGameResponse>> AtualizarAvaliacaoAsync(UserRatingRequest userRating, int userId, int ratingId)
    {
      var inputErrors = new List<Report>();
      var response = new Response<UserRatingGameResponse>(inputErrors);

      if (string.IsNullOrWhiteSpace(userId.ToString()))
        inputErrors.Add(Report.Create("Obrigatório fornecer usuário para avaliação.", 400));
      if (string.IsNullOrWhiteSpace(userRating.Rating.ToString()))
        inputErrors.Add(Report.Create("Obrigatório fornecer uma avaliação.", 400));
      if (userRating.Rating < 0 || userRating.Rating > 5)
        inputErrors.Add(Report.Create("Avaliação deve ser um número válido de 0 a 5.", 400));
      if (inputErrors.Count > 0)
        return response;

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      var rating = await _userRepository.BuscarAvaliacaoPorIdAsync(ratingId);
      if (rating == null)
      {
        response.Report.Add(Report.Create("Avaliação não encontrado.", 404));
        return response;
      }

      var game = await _gameRepository.BuscarPorIdAsync(rating.GameId);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!", 404));
        return response;
      }

      rating.Rating = userRating.Rating;
      rating.Comment = userRating.Comment;
      var ratingUpdated = await _userRepository.AtualizarAvaliacaoPorIdAsync(rating);
      if (ratingUpdated == null)
      {
        response.Report.Add(Report.Create("Erro interno ao retornar avaliação atualizada!", 500));
        return response;
      }

      response.Data = RatingMapper.ToUserGameResponse(ratingUpdated);
      _logger.LogInformation("Avaliação {RID} de jogo {GID}:{GNAME} atualizada por {UID}:{UNAME}", rating.Id, game.Id, game.Name, user.Id, user.Name);
      return response;
    }
    public async Task<Response<UserRatingListGamesResponse>> BuscarAvaliacoesAsync(int id)
    {
      var response = new Response<UserRatingListGamesResponse>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(id);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!", 404));
        return response;
      }

      var userRatings = await _userRepository.BuscarAvaliacoesDoUsuarioAsync(id);
      var totalRatings = userRatings.Count;

      response.Data = new UserRatingListGamesResponse
      {
        Ratings = userRatings
              .Select(ur => RatingMapper.ToUserGameResponse(ur)).ToList(),
        TotalRatings = totalRatings
      };

      return response;
    }
    public async Task<Response<string>> ExcluirAvaliacaoAsync(int userId, int ratingId)
    {
      var response = new Response<string>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado.", 404));
        return response;
      }

      var rating = await _userRepository.BuscarAvaliacaoPorIdAsync(ratingId);
      if (rating == null)
      {
        response.Report.Add(Report.Create("Avaliação não encontrada.", 404));
        return response;
      }

      var removed = await _userRepository.ExcluirAvaliacaoAsync(rating);
      if (!removed)
      {
        response.Report.Add(Report.Create("Erro ao excluir avaliação.", 500));
        return response;
      }

      response.Data = "Avaliação excluida com sucesso!";
      _logger.LogInformation("Avaliação {RID} de {UID}:{UNAME} excluida.", rating.Id, user.Id, user.Name);
      return response;
    }
  }
}
