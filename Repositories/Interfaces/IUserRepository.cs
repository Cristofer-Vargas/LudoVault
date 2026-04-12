using LudoVault.Model;

namespace LudoVault.Repositories.Interfaces
{
    public interface IUserRepository
    {
        UserModel CriarUsuario(UserModel user);
        UserModel Atualizar(UserModel user);
        UserModel BuscarUsuarioPorId(long id);
        bool VerificarEmailExistente(string email);
        bool VerificarIdExistente(long id);
    }
}
