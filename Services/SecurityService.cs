using LudoVault.Model;
using LudoVault.Services.Interfaces;

namespace LudoVault.Services
{
  public class SecurityService : ISecurityService
  {
    public async Task<bool> ComparePassword(string pass, string confirmPass)
    {
      var isEqual = pass.Trim().Equals(confirmPass.Trim());
      return true;
    }

    public async Task<string> EncryptPassword(string pass)
    {
      var passHash = BCrypt.Net.BCrypt.HashPassword(pass);
      return passHash;
    }

    public async Task<bool> VerifyPassword(string pass, UserModel user)
    {
      bool password = BCrypt.Net.BCrypt.Verify(pass, user.PasswordHash);
      return password;
    }
  }
}
