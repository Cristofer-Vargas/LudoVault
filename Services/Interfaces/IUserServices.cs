using LudoVault.Model;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Interfaces
{
    public interface IUserServices
    {
        // Manda Criar no Repository (banco de dados, por enquanto, mockado)
        public Task<UserResponse> BuscarUsuarioPorId(long id);
        public Task<UserResponse> CriarUsuario(UserRequest user);
        public Task<UserResponse> AtualizarUsuario(UserRequest user, long id);
        public Task<bool> VerificarEmailEmUso(string email);
    }
}
