using LudoVault.Model;

namespace LudoVault.Services.Interfaces
{
    public interface IUserServices
    {
        // Manda Criar no Repository (banco de dados, por enquanto, mockado)
        public Task<UserModel> BuscarUsuarioPorId(long id);
        public Task<bool> VerificarUserId(long id);
        public Task<UserModel> CriarUsuario(UserModel user);
        public Task<bool> VerificarEmailEmUso(string email);
    }
}
