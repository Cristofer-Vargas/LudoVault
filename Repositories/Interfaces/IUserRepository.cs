using LudoVault.Model;
using LudoVault.Services.Requests;

namespace LudoVault.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserModel> CriarUsuario(UserModel user);
        public Task<UserModel> Atualizar(UserModel user, long id);
        public Task<UserModel> BuscarUsuarioPorId(long id);
        public Task<List<GameRatingModel>> BuscarGamesComUserRatings(long id);
        public Task<bool> VerificarEmailExistente(string email);
    }
}
