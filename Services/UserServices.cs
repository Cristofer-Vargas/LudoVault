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
    public async Task<Response<UserResponse>> CriarUsuarioAsync(UserRequest user)
    {
      var inputErrors = new List<Report>();
      var response = new Response<UserResponse>(inputErrors);

      if (string.IsNullOrWhiteSpace(user.Email))
        inputErrors.Add(Report.Create("Email deve ser preenchido corretamente!", 400));
      if (string.IsNullOrWhiteSpace(user.Name))
        inputErrors.Add(Report.Create("Nome deve ser definido corretamente!", 400));
      if (string.IsNullOrWhiteSpace(user.PasswordHash))
        inputErrors.Add(Report.Create("Senha obrigatória deve ser preenchida corretamente!", 400));
      if (inputErrors.Count > 0)
        return response;

      if (await _userRepository.VerificarEmailExistenteAsync(null, user.Email ?? ""))
      {
        response.Report.Add(Report.Create("Email ja em uso!", 400));
        return response;
      }

      user.AvatarUrl = _sistema.CaminhoUserDefaultImage();
      user.PasswordHash = await _securityService.EncryptPassword(user.PasswordHash ?? "");
      var userModel = UserMapper.ToModel(user, user.PasswordHash);

      var currentUser = await _userRepository.CriarUsuarioAsync(userModel);
      if (currentUser == null)
      {
        _logger.LogError("Erro ao criar usuário {UNAME}.", user.Name);
        response.Report.Add(Report.Create("Erro ao criar usuário.", 500));
        return response;
      }

      _logger.LogInformation("Usuários {UNAME} criado com ID {UID}", currentUser.Name, currentUser.Id);
      var userResponse = UserMapper.ToResponse(currentUser);
      return Response.Ok(userResponse);
    }
    public async Task<Response<UserResponse>> AtualizarUsuarioAsync(UserRequest user, int id)
    {
      var inputErrors = new List<Report>();
      var response = new Response<UserResponse>(inputErrors);

      if (string.IsNullOrWhiteSpace(user.Email))
        inputErrors.Add(Report.Create("Email deve ser preenchido corretamente!", 400));

      if (string.IsNullOrWhiteSpace(user.Name))
        inputErrors.Add(Report.Create("Nome deve ser definido corretamente!", 400));

      if (inputErrors.Count > 0)
        return response;

      var userExisted = await _userRepository.BuscarUsuarioPorIdAsync(id);
      if (userExisted == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      if (!string.IsNullOrWhiteSpace(user.PasswordHash))
        user.PasswordHash = userExisted.PasswordHash;

      if (await _userRepository.VerificarEmailExistenteAsync(userExisted.Id, user.Email ?? ""))
      {
        response.Report.Add(Report.Create("Email ja em uso!"));
        return response;
      }

      userExisted.Name = user.Name ?? "";
      userExisted.Email = user.Email ?? "";
      userExisted.PasswordHash = await _securityService.EncryptPassword(user.PasswordHash ?? "");
      userExisted.Bio = user.Bio;

      var currentUser = await _userRepository.AtualizarUsuarioAsync(userExisted, id);
      if (currentUser == null)
      {
        _logger.LogError("Erro ao atualizar usuário {UNAME}", userExisted.Name);
        response.Report.Add(Report.Create("Erro ao atualizar usuário.", 500));
        return response;
      }

      var userRes = UserMapper.ToResponse(currentUser);
      _logger.LogInformation("Usuário {UNAME} com ID {UID} atualizado.", currentUser.Name, currentUser.Id);
      return Response.Ok(userRes);
    }
    public async Task<Response<UserResponse>> BuscarUsuarioPorIdAsync(int id)
    {
      var response = new Response<UserResponse>();

      var userResponse = UserMapper.ToResponse(await _userRepository.BuscarUsuarioPorIdAsync(id));
      if (userResponse == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      return Response.Ok(userResponse);
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
        if (!_imageServices.ExcluirImagemAsset(user.AvatarUrl) && !string.IsNullOrWhiteSpace(user.AvatarUrl))
        {
          response.Report.Add(Report.Create("Erro ao substituir imagem."));
          return response;
        }
        user.AvatarUrl = _sistema.CaminhoUserDefaultImage();
      }

      var caminho = await _imageServices.ConverteParaWebpESalvaImagem(image, "users");
      user.AvatarUrl = caminho;

      if (!await _userRepository.AtualizarImagemDePerfilAsync(user))
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
        if (!_imageServices.ExcluirImagemAsset(user.AvatarUrl ?? ""))
        {
          response.Report.Add(Report.Create("Não foi possivel excluir essa imagem!"));
          return response;
        }
      }

      user.AvatarUrl = _sistema.CaminhoUserDefaultImage();
      if (!await _userRepository.AtualizarImagemDePerfilAsync(user))
      {
        response.Report.Add(Report.Create("Erro ao atualizar imagem."));
        return response;
      }

      response.Data = user.AvatarUrl;
      return response;
    }

    public async Task<Response<string>> DeletarUsuarioAsync(int userId)
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
        if (!_imageServices.ExcluirImagemAsset(user.AvatarUrl))
        {
          response.Report.Add(Report.Create("Erro ao excluir imagem."));
          return response;
        }
      }
      user.AvatarUrl = _sistema.CaminhoUserDefaultImage();

      if (!await _userRepository.DeletarUsuarioAsync(user))
      {
        _logger.LogError("Erro ao excluir usuario {UNAME} com ID {UID}", user.Name, user.Id);
        response.Report.Add(Report.Create("Erro ao excluir usuário."));
        return response;
      }

      response.Data = "Usuário excluido com sucesso!";
      _logger.LogInformation("Usuario {UNAME} com ID {UID} excluido com sucesso!", user.Name, user.Id);
      return response;
    }


    // Listas de Usuário
    public async Task<Response<UserListListsResponse>> CriarUserListAsync(UserListRequest userList)
    {
      var response = new Response<UserListListsResponse>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userList.UserId);

      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var userListModel = UserListMapper.ToUserListModel(userList, userList.UserId);

      var existeListaComMesmoNome = await _userRepository.ExisteListaComMesmoNomeAsync(userListModel.Name ?? string.Empty, userList.UserId);
      if (existeListaComMesmoNome)
      {
        response.Report.Add(Report.Create("Existe outra lista com esse nome!"));
        return response;
      }

      var userCreatedModel = await _userRepository.CriarUserListAsync(userListModel);
      if (userCreatedModel == null)
      {
        response.Report.Add(Report.Create("Erro ao retornar lista recem criada!"));
        return response;
      }

      var userRes = UserListMapper.ToListGameResponse(userCreatedModel);
      _logger.LogInformation("Lista {LNAME} criada por {UNAME} de ID {UID}.", userCreatedModel.Name, user.Name, user.Id);
      return Response.Ok(userRes);
    }
    public async Task<Response<UserListResponse>> BuscarUserListsAsync(int id)
    {
      var response = new Response<UserListResponse>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(id);

      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var userLists = await _userRepository.BuscarUserListsPorUsuarioAsync(id);

      var totalLists = userLists.Count;

      var userListsResponse = new UserListResponse
      {
        Lists = userLists
                      .Select(ul => UserListMapper.ToListGameResponse(ul)).ToList(),
        TotalLists = totalLists
      };

      return Response.Ok(userListsResponse);

    }
    public async Task<Response<UserListListsResponse>> AtualizarUserListAsync(UserListRequest userList, int listId)
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

      var userListModel = UserListMapper.ToUserListModel(userList, userList.UserId);
      var existeListaComMesmoNome = await _userRepository.ExisteListaComMesmoNomeAsync(userListModel.Name ?? string.Empty, userList.UserId);
      if (existeListaComMesmoNome)
      {
        response.Report.Add(Report.Create("Existe outra lista com esse nome!"));
        return response;
      }

      var createdUserListModel = await _userRepository.AtualizarUserListAsync(userListModel, userList.UserId, listId);
      var userListRes = UserListMapper.ToListGameResponse(createdUserListModel);

      _logger.LogInformation("Lista de {UID}:{UNAME} com list ID {LID} atualizada para {LNEWNAME}", user.Id, user.Name, list.Id, userList.Name); 
      return Response.Ok(userListRes);
    }
    public async Task<Response<UserListListsResponse>> CriarGameInListAsync(UserListGameRequest userGameList, int userId)
    {
      var response = new Response<UserListListsResponse>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var userListGameModel = UserListMapper.ToUserListGameModel(userGameList, userGameList.ListId);

      if (!await _gameRepository.GameExisteAsync(userGameList.GameId))
      {
        response.Report.Add(Report.Create("Não foi possivel encontrar esse jogo."));
        return response;
      }

      var gameExistInThisList = await _userRepository.JogoExisteNaListaAsync(userGameList.GameId, userGameList.ListId);
      if (gameExistInThisList)
      {
        response.Report.Add(Report.Create("Esse jogo ja foi adicionado!"));
        return response;
      }

      var userListGame = await _userRepository.AdicionarJogoAListaAsync(userListGameModel);
      if (userListGame == null)
      {
        _logger.LogError("Erro ao retornar lista para o usuário {UID} após adicionar jogo {GID}.", userId, userGameList.GameId);
        response.Report.Add(Report.Create("Erro ao retornar essa lista."));
        return response;
      }

      var userListRes = UserListMapper.ToListGameResponse(userListGame);
      _logger.LogInformation("Jogo {GID} adicionado em {LNAME} de {UNAME}.", userGameList.GameId, userListGame.Name, user.Name);
      return Response.Ok(userListRes);
    }
    public async Task<Response<string>> DeletarUserListAsync(int userId, int listId)
    {
      var response = new Response<string>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var userListExist = await _userRepository.ExisteUserListAsync(listId, userId);
      if (!userListExist)
      {
        response.Data = "Erro ao excluir a lista.";
        return response;
      }

      await _userRepository.DeletarUserListAsync(listId);
      response.Data = "Lista excluída com sucesso!";
      _logger.LogInformation("Lista {LID} excluida de {UNAME}.", listId, user.Name);
      return response;
    }
    public async Task<Response<string>> DeletarGameInUserListAsync(int userId, int listId, int gameId)
    {
      var response = new Response<string>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var gameExistInUserList = await _userRepository.JogoExisteNaListaAsync(gameId, listId);
      if (gameExistInUserList)
      {
        await _userRepository.RemoverJogoDaListaAsync(listId, gameId);
        response.Data = "Removido com sucesso!";
        _logger.LogInformation("Jogo {GID} removido da lista {LID} por {UNAME}", gameId, listId, user.Name);
        return response;
      }

      response.Data = "Erro ao remover jogo.";
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

      if (!await _gameRepository.GameExisteAsync(userLibrary.GameId))
      {
        response.Report.Add(Report.Create("Não foi possivel encontrar esse jogo."));
        return response;
      }

      var gameExist = await _userRepository.ExisteJogoNaBibliotecaAsync(userLibrary.UserId, userLibrary.GameId);
      if (gameExist)
      {
        response.Report.Add(Report.Create("Esse jogo ja foi adicionado a biblioteca."));
        return response;
      }

      var userLibraryModel = UserLibraryMapper.ToModel(userLibrary);
      var IsAdded = await _userRepository.AdicionarJogoABibliotecaAsync(userLibraryModel);
      if (IsAdded)
      {
        _logger.LogInformation("Jogo {GID} adicionado a biblioteca de {UNAME}", userLibrary.GameId, user.Name);
        response.Data = "Jogo adicionado com sucesso!";
        return response;
      }

      response.Report.Add(Report.Create("Não foi possivel adicionar a biblioteca."));
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

      var libraryGamesResponse = libraryGames
              .Select(lg => UserLibraryMapper.ToGameResponse(lg))
              .ToList();

      return Response.Ok(libraryGamesResponse);
    }
    public async Task<Response<string>> RemoverJogoDaBibliotecaAsync(int userId, int libraryId)
    {
      var response = new Response<string>();
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado!"));
        return response;
      }

      var removido = await _userRepository.RemoverJogoDaBibliotecaAsync(libraryId);
      if (!removido)
      {
        response.Report.Add(Report.Create("Não foi possível remover o jogo ou registro não encontrado."));
        return response;
      }

      response.Data = "Removido com sucesso!";
      //_logger.LogInformation("Jogo {GID} removido da biblioteca de {UNAME}", userLibrary.GameId, user.Name);
      return response;
    }

    // Avaliações de Usuário
    public async Task<Response<UserRatingListGamesResponse>> BuscarUserRatingsAsync(int id)
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

      var userRatingsResponse = new UserRatingListGamesResponse
      {
        Ratings = userRatings
              .Select(ur => RatingMapper.ToUserGameResponse(ur)).ToList(),
        TotalRatings = totalRatings
      };

      return Response.Ok(userRatingsResponse);
    }

    public async Task<Response<UserRatingListGamesResponse>> AdicionarUserRatingAsync(UserRatingRequest userRating, int userId, int gameId)
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
      var game = await _gameRepository.BuscarGamePorIdAsync(gameId);

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
      if (await _userRepository.BuscarRatingPorUserEGameAsync(userId, gameId) != null)
      {
        response.Report.Add(Report.Create("Esse jogo ja foi avaliado!"));
        return response;
      }

      var userRatingModel = RatingMapper.ToRatingModel(userRating, user, game);
      var userRatings = await _userRepository.AdicionarUserRatingAsync(userRatingModel);

      var totalRatings = userRatings.Count;

      var userRatingsResponse = new UserRatingListGamesResponse
      {
        Ratings = userRatings
              .Select(ur => RatingMapper.ToUserGameResponse(ur)).ToList(),
        TotalRatings = totalRatings
      };

      _logger.LogInformation("Avaliação de {UNAME} adicinada para {GNAME}", user.Name, game.Name);
      return Response.Ok(userRatingsResponse);
    }

    public async Task<Response<UserRatingGameResponse>> BuscarRatingPorIdAsync(int ratingId)
    {
      var response = new Response<UserRatingGameResponse>();

      var rating = await _userRepository.BuscarRatingPorIdAsync(ratingId);
      if (rating == null)
      {
        response.Report.Add(Report.Create("Erro ao encontrar avaliação.", 400));
        return response;
      }

      return response;
    }

    public async Task<Response<UserRatingGameResponse>> AtualizarRatingAsync(UserRatingRequest userRating, int userId, int ratingId)
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

      var ratingExist = await _userRepository.BuscarRatingPorIdAsync(ratingId);
      if (ratingExist == null)
      {
        response.Report.Add(Report.Create("Avaliação não encontrado."));
        return response;
      }

      var ratingModel = RatingMapper.ToRatingModel(userRating, user, ratingExist.Game);

      var rating = await _userRepository.AtualizarRatingPorIdAsync(ratingModel, ratingId);

      response.Data = RatingMapper.ToUserGameResponse(rating);
      _logger.LogInformation("Avaliação {RID} de jogo {GID} atualizada por {UNAME}", ratingId, ratingExist.GameId, user.Name);
      return response;
    }

    public async Task<Response<string>> RemoverRatingAsync(int userId, int ratingId)
    {
      var response = new Response<string>();

      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);
      if (user == null)
      {
        response.Report.Add(Report.Create("Usuário não encontrado.", 400));
        return response;
      }

      var rating = await _userRepository.BuscarRatingPorIdAsync(ratingId);
      if (rating == null)
      {
        response.Report.Add(Report.Create("Avaliação não encontrada.", 400));
        return response;
      }

      var removed = await _userRepository.RemoverRatingAsync(rating);
      if (removed == false)
      {
        response.Report.Add(Report.Create("Erro ao excluir avaliação.", 500));
        return response;
      }

      response.Data = "Avaliação excluida com sucesso!";
      _logger.LogInformation("Avaliação {RID} de {UNAME} removida!", ratingId, user.Name);
      return response;
    }
  }
}
