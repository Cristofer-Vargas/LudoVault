using LudoVault.Model;

namespace LudoVault.Services.Interfaces
{
  public interface ISecurityService
  {
    // Comparar, Criar e verificar criptografia por BCrypt
    public Task<bool> ComparePassword(string pass, string confirmPass);
    public Task<bool> VerifyPassword(string pass, UserModel user);
    public Task<string> EncryptPassword(string pass);
  }
}
