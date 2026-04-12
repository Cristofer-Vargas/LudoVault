using LudoVault.Model;

namespace LudoVault.Services.Interfaces
{
    public interface IUserServices
    {
        // Manda Criar no Repository (banco de dados, por enquanto, mockado)
        UserModel BuscarUsuarioPorId(long id);
        bool VerificarUserId(long id);
        UserModel CriarUsuario(UserModel user);
        bool VerificarEmailEmUso(string email);
    }
}
