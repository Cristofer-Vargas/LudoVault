using LudoVault.Model;

namespace LudoVault.Services.Interfaces
{
    public interface ISecurityService
    {
        // Comparar, Criar e verificar criptografia por BCrypt
        public bool ComparePassword(string pass, string confirmPass);
        public bool VerifyPassword(string pass, UserModel user);
        public string EncryptPassword(string pass);
    }
}
