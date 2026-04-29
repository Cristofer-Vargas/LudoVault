using LudoVault.Model;
using LudoVault.Services.Requests;

namespace LudoVault.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserModel> CriarUsuario(UserModel user);
        public Task<UserModel> Atualizar(UserModel user, int id);
        public Task<UserModel> BuscarUsuarioPorId(int id);
        public Task<List<GameRatingModel>> BuscarGamesComUserRatings(int id);
        public Task<List<UserListModel>> BuscarUserLists(int id);
        public Task<bool> VerificarEmailExistente(string email);
    }
}
