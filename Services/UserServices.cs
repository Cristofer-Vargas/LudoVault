using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services
{
    public class UserServices(IUserRepository userRepo, ISecurityService securityService) : IUserServices
    {
        private readonly IUserRepository _userRepository = userRepo;
        private readonly ISecurityService _securityService = securityService;

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

            userExisted.Name = user.Name;
            userExisted.Email = user.Email;
            userExisted.PasswordHash = await _securityService.EncryptPassword(user.PasswordHash);
            userExisted.Bio = user.Bio;

            UserResponse userRes =  UserMapper.ToResponse(await _userRepository.AtualizarUsuarioAsync(userExisted, id));
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