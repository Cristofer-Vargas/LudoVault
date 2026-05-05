using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;

namespace LudoVault.Services
{
  public class UserServices(IUserRepository userRepo, ISecurityService securityService, 
    IGameRepository gameRepo, IImageServices imageServices, ISystemServices sistema) : IUserServices
  {
    private readonly IUserRepository _userRepository = userRepo;
    private readonly IGameRepository _gameRepository = gameRepo;
    private readonly ISecurityService _securityService = securityService;
    private readonly IImageServices _imageServices = imageServices;
    private readonly ISystemServices _sistema = sistema;

    // Usuário
    public async Task<UserResponse> CriarUsuarioAsync(UserRequest user)
    {
      user.PasswordHash = await _securityService.EncryptPassword(user.PasswordHash);
      var userModel = UserMapper.ToModel(user, user.PasswordHash);

      UserResponse userResponse = UserMapper.ToResponse(await _userRepository.CriarUsuarioAsync(userModel));
      return userResponse;
    }
    public async Task<UserResponse> AtualizarUsuarioAsync(UserRequest user, int id)
    {
      var userExisted = await _userRepository.BuscarUsuarioPorIdAsync(id);

      userExisted.Name = user.Name ?? "";
      userExisted.Email = user.Email ?? "";
      userExisted.PasswordHash = await _securityService.EncryptPassword(user.PasswordHash);
      userExisted.Bio = user.Bio;

      UserResponse userRes = UserMapper.ToResponse(await _userRepository.AtualizarUsuarioAsync(userExisted, id));
      return userRes;
    }
    public async Task<UserResponse> BuscarUsuarioPorIdAsync(int id)
    {
      var userResponse = UserMapper.ToResponse(await _userRepository.BuscarUsuarioPorIdAsync(id));
      return userResponse;
    }
    public async Task<bool> VerificarEmailEmUsoAsync(string email)
    {
      bool emailJaExiste = await _userRepository.VerificarEmailExistenteAsync(email);
      if (emailJaExiste) return true;
      return false;
    }
    public async Task<string> AdicionarImagemDePerfilAsync(IFormFile image, int userId)
    {
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);

      var caminho = await _imageServices.ConverteParaWebpESalvaImagem(image, "users");
      user.AvatarUrl = caminho;
      await _userRepository.AtualizarImagemDePerfilAsync(user);
      return caminho;
    }
    public async Task<string> RemoverImagemDePerfilAsync(int userId)
    {
      var user = await _userRepository.BuscarUsuarioPorIdAsync(userId);

      if ( ! _imageServices.ExcluirImagemAsset(user.AvatarUrl ?? "")) 
        throw new ArgumentException("Não foi possivel excluir essa imagem!");

      user.AvatarUrl = Path.Combine(_sistema.CaminhoAssetsRoot(), "uploads", "users", "default-image.webp");
      await _userRepository.AtualizarImagemDePerfilAsync(user);
      return user.AvatarUrl;
    }


    // Listas de Usuário
    public async Task<UserListListsResponse> CriarUserListAsync(UserListRequest userList)
    {
      await _userRepository.BuscarUsuarioPorIdAsync(userList.UserId);
      var userListModel = UserListMapper.ToUserListModel(userList, userList.UserId);

      var existeListaComMesmoNome = await _userRepository.ExisteListaComMesmoNomeAsync(userListModel.Name ?? string.Empty, userList.UserId);
      if (existeListaComMesmoNome)
        throw new ArgumentException("Existe outra lista com esse nome!");

      var userCreatedModel = await _userRepository.CriarUserListAsync(userListModel);

      return UserListMapper.ToListGameResponse(userCreatedModel);
    }
    public async Task<UserListResponse> BuscarUserListsAsync(int id)
    {
      await _userRepository.BuscarUsuarioPorIdAsync(id);

      var userLists = await _userRepository.BuscarUserListsPorUsuarioAsync(id);

      var totalLists = userLists.Count;

      var userListsResponse = new UserListResponse
      {
        Lists = userLists
                      .Select(ul => UserListMapper.ToListGameResponse(ul)).ToList(),
        TotalLists = totalLists
      };

      return userListsResponse;

    }
    public async Task<UserListListsResponse> AtualizarUserListAsync(UserListRequest userList, int listId)
    {
      await _userRepository.BuscarUsuarioPorIdAsync(userList.UserId);

      var userListModel = UserListMapper.ToUserListModel(userList, userList.UserId);
      var existeListaComMesmoNome = await _userRepository.ExisteListaComMesmoNomeAsync(userListModel.Name ?? string.Empty, userList.UserId);
      if (existeListaComMesmoNome)
        throw new ArgumentException("Existe outra lista com esse nome!");

      var createdUserListModel = await _userRepository.AtualizarUserListAsync(userListModel, userList.UserId, listId);

      return UserListMapper.ToListGameResponse(createdUserListModel);
    }
    public async Task<UserListListsResponse> CriarGameInListAsync(UserListGameRequest userGameList, int userId)
    {
      await _userRepository.BuscarUsuarioPorIdAsync(userId);
      var userListGameModel = UserListMapper.ToUserListGameModel(userGameList, userGameList.ListId);

      if (!await _gameRepository.GameExisteAsync(userGameList.GameId))
        throw new ArgumentException("Não foi possivel encontrar esse jogo.");

      var gameExistInThisList = await _userRepository.JogoExisteNaListaAsync(userGameList.GameId, userGameList.ListId);
      if (gameExistInThisList)
        throw new ArgumentException("Esse jogo ja foi adicionado!");

      var userListGame = await _userRepository.AdicionarJogoAListaAsync(userListGameModel);

      return UserListMapper.ToListGameResponse(userListGame);
    }
    public async Task<bool> DeletarUserListAsync(int userId, int listId)
    {
      await _userRepository.BuscarUsuarioPorIdAsync(userId);
      var userListExist = await _userRepository.ExisteUserListAsync(listId, userId);
      if (userListExist)
      {
        await _userRepository.DeletarUserListAsync(listId);
        return true;
      }

      return false;
    }
    public async Task<bool> DeletarGameInUserListAsync(int userId, int listId, int gameId)
    {
      await _userRepository.BuscarUsuarioPorIdAsync(userId);
      var gameExistInUserList = await _userRepository.JogoExisteNaListaAsync(gameId, listId);
      if (gameExistInUserList)
      {
        await _userRepository.RemoverJogoDaListaAsync(listId, gameId);
        return true;
      }

      return false;
    }

    // Biblioteca de Usuário
    public async Task<bool> AdicionarJogoABibliotecaAsync(UserLibraryRequest userLibrary)
    {
      var userLibraryModel = UserLibraryMapper.ToModel(userLibrary);

      if (!await _gameRepository.GameExisteAsync(userLibrary.GameId))
        throw new ArgumentException("Não foi possivel encontrar esse jogo.");

      var gameExist = await _userRepository.ExisteJogoNaBiblioteca(userLibrary.UserId, userLibrary.GameId);
      if (!gameExist)
      {
        var IsAdded = await _userRepository.AdicionarJogoABibliotecaAsync(userLibraryModel);
        if (IsAdded)
          return true;

        throw new ArgumentException("Não foi possivel adicionar a biblioteca.");
      }
      else
      {
        throw new ArgumentException("Esse jogo ja foi adicionado a biblioteca.");
      }
    }
    public async Task<List<UserLibraryGameResponse>> BuscarJogosDaBiblioteca(int id)
    {
      var libraryGames = await _userRepository.BuscarJogosDaBiblioteca(id);

      var libraryGamesResponse = libraryGames
              .Select(lg => UserLibraryMapper.ToGameResponse(lg))
              .ToList();

      return libraryGamesResponse;
    }
    public async Task<bool> RemoverJogoDaBiblioteca(int libraryId)
    {
      return await _userRepository.RemoverJogoDaBiblioteca(libraryId);
    }

    // Avaliações de Usuário
    public async Task<UserRatingListGamesResponse> BuscarUserRatingsAsync(int id)
    {
      var userRatings = await _userRepository.BuscarAvaliacoesDoUsuarioAsync(id);
      if (userRatings.Count == 0)
        throw new ArgumentException("Nenhuma avaliação desse usuário!");

      var totalRatings = userRatings.Count;

      var userRatingsResponse = new UserRatingListGamesResponse
      {
        GamesRatings = userRatings
              .Select(ur => GameRatingMapper.ToUserGameResponse(ur)).ToList(),
        TotalRatings = totalRatings
      };

      return userRatingsResponse;
    }
  }
}