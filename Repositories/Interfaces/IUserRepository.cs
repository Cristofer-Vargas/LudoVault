using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserModel> CriarUsuario(UserModel user);
        public Task<UserModel> Atualizar(UserModel user);
        public Task<UserModel> BuscarUsuarioPorId(long id);
        public Task<bool> VerificarEmailExistente(string email);
        public Task<bool> VerificarIdExistente(long id);
    }
}
