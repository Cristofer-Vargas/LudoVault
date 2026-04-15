using LudoVault.Model;
using LudoVault.Services.Interfaces;

namespace LudoVault.Services
{
    public class SecurityService : ISecurityService
    {
        public Task<bool> ComparePassword(string pass, string confirmPass)
        {
            var isEqual = pass.Trim().Equals(confirmPass.Trim());
            return Task.FromResult(true);
        }

        public Task<string> EncryptPassword(string pass)
        {
            var passHash = BCrypt.Net.BCrypt.HashPassword(pass);
            return Task.FromResult(passHash);
        }

        public Task<bool> VerifyPassword(string pass,UserModel user)
        {
            bool password = BCrypt.Net.BCrypt.Verify(pass, user.PasswordHash);
            return Task.FromResult(password);
        }
    }
}
