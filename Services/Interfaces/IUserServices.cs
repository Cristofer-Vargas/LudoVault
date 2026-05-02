using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Model;

namespace LudoVault.Services.Interfaces
{
    public interface IUserServices
    {
        // Usuário
        public Task<UserResponse> CriarUsuarioAsync(UserRequest user);
        public Task<UserResponse> AtualizarUsuarioAsync(UserRequest user, int id);
        public Task<UserResponse> BuscarUsuarioPorIdAsync(int id);
        public Task<bool> VerificarEmailEmUsoAsync(string email);

        // Listas de Usuário
        public Task<UserListListsResponse> CriarUserListAsync(UserListRequest userList);
        public Task<UserListListsResponse> AtualizarUserListAsync(UserListRequest userList, int listId);
        public Task<UserListListsResponse> CriarGameInListAsync(UserListGameRequest userGameList, int userId);
        public Task<UserListResponse> BuscarUserListsAsync(int id);
        public Task<bool> DeletarUserListAsync(int userId, int listId);
        public Task<bool> DeletarGameInUserListAsync(int userId, int listId, int gameId);

        // Avaliações de Usuários
        public Task<UserRatingListGamesResponse> BuscarUserRatingsAsync(int id);
    }
}
