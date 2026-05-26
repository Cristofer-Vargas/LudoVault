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
        response.Report.Add(Report.Create("Email ja em uso!", 400));
        return response;
      }

      userRequest.AvatarUrl = _sistema.CaminhoUserDefaultImage();
      userRequest.PasswordHash = await _securityService.EncryptPassword(userRequest.PasswordHash ?? "");
      var userModel = UserMapper.ToModel(userRequest, userRequest.PasswordHash);

      var currentUser = await _userRepository.CriarUsuarioAsync(userModel);
      if (currentUser == null)
      {
        _logger.LogError("Erro ao criar usuário {UNAME}.", userRequest.Name);
        response.Report.Add(Report.Create("Erro ao criar usuário.", 500));
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
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      if (!string.IsNullOrWhiteSpace(userRequest.PasswordHash))
        userRequest.PasswordHash = user.PasswordHash;

      var emailExist = await _userRepository.VerificarEmailExistenteAsync(userID, userRequest.Email.Trim());
      if (emailExist)
      {
        response.Report.Add(Report.Create("Email em uso!", 400));
        return response;
      }

      user.Name = userRequest.Name ?? "";
      user.Email = userRequest.Email ?? "";
      user.PasswordHash = await _securityService.EncryptPassword(userRequest.PasswordHash ?? "");
      user.Bio = userRequest.Bio;

      var currentUser = await _userRepository.AtualizarUsuarioAsync(user);
      if (currentUser == null)
      {
        _logger.LogError("Erro ao atualizar usuário {UNAME}", user.Name);
        response.Report.Add(Report.Create("Erro ao atualizar usuário.", 500));
        return response;
      }

      _logger.LogInformation("Usuário {UNAME} com ID {UID} atualizado.", currentUser.Name, currentUser.Id);
      response.Data = UserMapper.ToResponse(currentUser);
      return response;
    }
    public async Task<Response<UserResponse>> BuscarUsuarioPorIdAsync(int id)
    {
      var response = new Response<UserResponse>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(id);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      response.Data = UserMapper.ToResponse(user);
      return response;
    }
    public async Task<Response<string>> AdicionarImagemDePerfilAsync(IFormFile image, int userId)
    {
      var response = new Response<string>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      if (user.AvatarUrl != _sistema.CaminhoUserDefaultImage())
      {
        var imageDeleted = _imageServices.ExcluirImagemAsset(user.AvatarUrl);
        if (!imageDeleted && !string.IsNullOrWhiteSpace(user.AvatarUrl))
        {
          response.Report.Add(Report.Create("Erro ao substituir imagem."));
          return response;
        }
        user.AvatarUrl = _sistema.CaminhoUserDefaultImage();
      }

      var caminho = await _imageServices.ConverteParaWebpESalvaImagem(image, "users");
      user.AvatarUrl = caminho;
      var imgUpdated = await _userRepository.AtualizarImagemDePerfilAsync(user);
      if (!imgUpdated)
      {
        _logger.LogError("Erro ao atualizar imagem {UIMG} de usuario {UNAME} com ID {UID}", caminho, user.Name, user.Id);
        response.Report.Add(Report.Create("Erro ao atualizar imagem."));
        return response;
      }

      response.Data = caminho;
      _logger.LogInformation("Imagem de {UNAME} com ID {UID} atualizada com sucesso.", user.Name, user.Id);
      return response;
    }
    public async Task<Response<string>> RemoverImagemDePerfilAsync(int userId)
    {
      var response = new Response<string>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      _logger.LogInformation("Removendo imagem de {UNAME} com ID {UID}.", user.Name, user.Id);
      if (user.AvatarUrl != _sistema.CaminhoUserDefaultImage())
      {
        var imageDeleted = _imageServices.ExcluirImagemAsset(user.AvatarUrl ?? "");
        if (!imageDeleted)
        {
          response.Report.Add(Report.Create("Não foi possivel excluir essa imagem!"));
          return response;
        }
      }

      user.AvatarUrl = _sistema.CaminhoUserDefaultImage();
      var imgUpdated = await _userRepository.AtualizarImagemDePerfilAsync(user);
      if (!imgUpdated)
      {
        response.Report.Add(Report.Create("Erro ao atualizar imagem."));
        return response;
      }

      response.Data = user.AvatarUrl;
      return response;
    }
    public async Task<Response<string>> ExcluirUsuarioAsync(int userId)
    {
      var response = new Response<string>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      if (user.AvatarUrl != _sistema.CaminhoUserDefaultImage() && !string.IsNullOrWhiteSpace(user.AvatarUrl))
      {
        var imageDeleted = _imageServices.ExcluirImagemAsset(user.AvatarUrl);
        if (!imageDeleted)
        {
          response.Report.Add(Report.Create("Erro ao excluir imagem."));
          return response;
        }
      }
      user.AvatarUrl = _sistema.CaminhoUserDefaultImage();

      var userExcluded = await _userRepository.ExcluirUsuarioAsync(user);
      if (!userExcluded)
      {
        _logger.LogError("Erro ao excluir usuario {UID}:{UNAME}!", user.Id, user.Name);
        response.Report.Add(Report.Create("Erro ao excluir usuário."));
        return response;
      }

      response.Data = "Usuário excluido com sucesso!";
      _logger.LogInformation("Usuario {UNAME} com ID {UID} excluido com sucesso!", user.Name, user.Id);
      return response;
    }

    // Listas de Usuário
    public async Task<Response<UserListListsResponse>> CriarListaAsync(UserListRequest userList)
    {
      var response = new Response<UserListListsResponse>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userList.UserId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var existeListaComMesmoNome = await _userRepository.ExisteListaComMesmoNomeAsync(userList.Name ?? string.Empty, userList.UserId);
      if (existeListaComMesmoNome)
      {
        response.Report.Add(Report.Create("Existe outra lista com esse nome!"));
        return response;
      }

      var userListModel = UserListMapper.ToUserListModel(userList, userList.UserId);
      var lista = await _userRepository.CriarListaAsync(userListModel);
      if (lista == null)
      {
        response.Report.Add(Report.Create("Erro ao retornar lista recem criada!"));
        return response;
      }

      response.Data = UserListMapper.ToListGameResponse(lista);
      _logger.LogInformation("Lista {LNAME} criada por {UID}:{UNAME}.", lista.Name, user.Id, user.Name);
      return response;
    }
    public async Task<Response<UserListListsResponse>> AtualizarListaAsync(UserListRequest userList, int listId)
    {
      var response = new Response<UserListListsResponse>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userList.UserId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var list = await _userRepository.BuscarListaAsync(listId);
      if (list == null)
      {
        response.Report.Add(Report.Create("Lista não encontrada!"));
        return response;
      }

      var existeListaComMesmoNome = await _userRepository.ExisteListaComMesmoNomeAsync(userList.Name ?? string.Empty, userList.UserId);
      if (existeListaComMesmoNome)
      {
        response.Report.Add(Report.Create("Existe outra lista com esse nome!"));
        return response;
      }

      var listModel = UserListMapper.ToUserListModel(userList, userList.UserId);
      listModel.Id = list.Id;
      listModel.UserId = user.Id;
      listModel.CreatedAt = list.CreatedAt;

      var createdListModel = await _userRepository.AtualizarListaAsync(listModel);
      _logger.LogInformation("Lista {LID}:{OLDLNAME} de {UID}:{UNAME} atualizada para {LNEWNAME}.", list.Id, list.Name, user.Id, user.Name, createdListModel.Name);
      response.Data = UserListMapper.ToListGameResponse(createdListModel);
      return response;
    }
    public async Task<Response<UserListListsResponse>> AdicionarJogoAListaAsync(UserListGameRequest userGameList, int userId)
    {
      var response = new Response<UserListListsResponse>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var list = await _userRepository.BuscarListaAsync(userGameList.ListId);
      if (list == null)
      {
        response.Report.Add(Report.Create("Lista não encontrada!"));
        return response;
      }

      var game = await _gameRepository.BuscarPorIdAsync(userGameList.GameId);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      var gameExistInList = await _userRepository.BuscarJogoDaListaAsync(userGameList.GameId, userGameList.ListId);
      if (gameExistInList != null)
      {
        response.Report.Add(Report.Create("Esse jogo ja foi adicionado!"));
        return response;
      }

      var userListGameModel = UserListMapper.ToUserListGameModel(userGameList);
      var userListGame = await _userRepository.AdicionarJogoAListaAsync(userListGameModel);
      if (userListGame == null)
      {
        _logger.LogError("Erro ao retornar lista {LID}:{LNAME} para o usuário {UID}:{UNAME} após adicionar jogo {GID}:{GNAME}.", list.Id, list.Name, user.Id, user.Name, game.Id, game.Name);
        response.Report.Add(Report.Create("Erro ao retornar lista recém criada."));
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
        response.Report.Add(Report.Create("Usuário não encontrado!"));
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
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var list = await _userRepository.BuscarListaAsync(listId);
      if (list == null)
      {
        response.Report.Add(Report.Create("Lista não econtrada."));
        return response;
      }

      var listExcluded = await _userRepository.ExcluirListaAsync(list);
      if (!listExcluded)
      {
        _logger.LogError("Erro ao excluir a lista {LID}:{LNAME} de {UID}:{UNAME}", list.Id, list.Name, user.Id, user.Name);
        response.Report.Add(Report.Create("Erro ao excluir a lista!"));
        return response;
      }

      response.Data = "Lista excluída com sucesso!";
      _logger.LogInformation("Lista {LID}:{LNAME} excluida por {UID}:{UNAME}.", list.Id, list.Name, user.Id, user.Name);
      return response;
    }
    public async Task<Response<string>> RemoverJogoDeListaAsync(int userId, int listId, int gameId)
    {
      var response = new Response<string>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var game = await _gameRepository.BuscarPorIdAsync(gameId);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      var list = await _userRepository.BuscarListaAsync(listId);
      if (list == null)
      {
        response.Report.Add(Report.Create("Lista não econtrada."));
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
        response.Report.Add(Report.Create("Erro ao remover jogo da lista!", 400));
        return response;
      }

      response.Data = "Removido com sucesso!";
      _logger.LogInformation("Jogo {GID}:{GNAME} removido da lista {LID}:{LNAME} por {UID}:{UNAME}", game.Id, game.Name, list.Id, list.Name, user.Id, user.Name);
      return response;
    }

    // Biblioteca de Usuário
    public async Task<Response<string>> AdicionarJogoABibliotecaAsync(UserLibraryRequest userLibrary)
    {
      var response = new Response<string>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userLibrary.UserId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado."));
        return response;
      }

      var game = await _gameRepository.BuscarPorIdAsync(userLibrary.GameId);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      var libraryGame = await _userRepository.BuscarPorIdJogoDaBibliotecaAsync(userLibrary.UserId, userLibrary.GameId);
      if (libraryGame == null)
      {
        response.Report.Add(Report.Create("Esse jogo ja foi adicionado a biblioteca.", 400));
        return response;
      }

      var userLibraryModel = UserLibraryMapper.ToModel(userLibrary);
      var IsAdded = await _userRepository.AdicionarJogoNaBibliotecaAsync(userLibraryModel);
      if (!IsAdded)
      {
        response.Report.Add(Report.Create("Não foi possivel adicionar a biblioteca.", 500));
        return response;
      }

      _logger.LogInformation("Jogo {GID}:{GNAME} adicionado a biblioteca de {UID}:{UNAME}", game.Id, game.Name, user.Id, user.Name);
      response.Data = "Jogo adicionado com sucesso!";
      return response;
    }
    public async Task<Response<List<UserLibraryGameResponse>>> BuscarJogosDaBibliotecaAsync(int id)
    {
      var response = new Response<List<UserLibraryGameResponse>>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(id);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var libraryGames = await _userRepository.BuscarJogosDaBibliotecaAsync(id);

      response.Data = libraryGames
              .Select(lg => UserLibraryMapper.ToGameResponse(lg))
              .ToList();

      return response;
    }
    public async Task<Response<string>> RemoverJogoDaBibliotecaAsync(int userId, int gameId)
    {
      var response = new Response<string>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var game = await _gameRepository.BuscarPorIdAsync(gameId);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      var libraryGame = await _userRepository.BuscarPorIdJogoDaBibliotecaAsync(userId, gameId);
      if (libraryGame == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado na bilbioteca.", 404));
        return response;
      }

      var removido = await _userRepository.RemoverJogoDaBibliotecaAsync(libraryGame);
      if (!removido)
      {
        response.Report.Add(Report.Create("Não foi possível remover o jogo da biblioteca."));
        return response;
      }

      response.Data = "Removido com sucesso!";
      _logger.LogInformation("Jogo {GID}:{GNAME} removido da biblioteca de {UID}:{UNAME}", game.Id, game.Name, user.Id, user.Name);
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
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;

      }
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }
      
      var rating = await _userRepository.BuscarAvaliacaoPorUserEGameAsync(userId, gameId);
      if (rating != null)
      {
        response.Report.Add(Report.Create("Esse jogo ja foi avaliado!"));
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

      _logger.LogInformation("Avaliação de {GID}:{GNAME} adicinada por {UID}:{UNAME}", game.Id, game.Name, user.Id, user.Name);
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
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var rating = await _userRepository.BuscarAvaliacaoPorIdAsync(ratingId);
      if (rating == null)
      {
        response.Report.Add(Report.Create("Avaliação não encontrado."));
        return response;
      }

      var game = await _gameRepository.BuscarPorIdAsync(rating.GameId);
      if (game == null)
      {
        response.Report.Add(Report.Create("Jogo não encontrado!"));
        return response;
      }

      var ratingModel = RatingMapper.ToRatingModel(userRating, user, rating.Game);
      var ratingUpdated = await _userRepository.AtualizarAvaliacaoPorIdAsync(ratingModel);
      if (ratingUpdated == null)
      {
        response.Report.Add(Report.Create("Erro ao retornar avaliação atualizada!", 500));
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
        response.Report.Add(Report.Create("Usuário não encontrado!"));
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
