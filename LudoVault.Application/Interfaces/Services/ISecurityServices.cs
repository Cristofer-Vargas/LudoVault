using LudoVault.Domain.Model;

namespace LudoVault.Application.Interfaces.Services
{
  public interface ISecurityServices
  {
    // Comparar, Criar e verificar criptografia por BCrypt
    public Task<bool> ComparePassword(string pass, string confirmPass);
    public Task<bool> VerifyPassword(string pass, UserModel user);
    public Task<string> EncryptPassword(string pass);
  }
}
