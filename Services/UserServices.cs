using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;

namespace LudoVault.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityService _securityService;

        public UserServices(IUserRepository userRepo, ISecurityService securityService)
        {
            _userRepository = userRepo;
            _securityService = securityService;
        }

        public async Task<UserModel> BuscarUsuarioPorId(long id)
        {
            return await _userRepository.BuscarUsuarioPorId(id);
        }

        public async Task<UserModel> CriarUsuario(UserModel user)
        {
            // Hash na senha passada pelo usuário
            user.PasswordHash = await _securityService.EncryptPassword(user.PasswordHash);
            UserModel newUser = await _userRepository.CriarUsuario(user);
            return newUser;
        }

        public async Task<bool> VerificarEmailEmUso(string email)
        {
            // Verificar se há usuario com email existente
            bool emailJaExiste = await _userRepository.VerificarEmailExistente(email);
            if (emailJaExiste) return true;
            return false;
        }

        public async Task<bool> VerificarUserId(long id)
        {
            bool idExistente = await _userRepository.VerificarIdExistente(id);
            if (idExistente) return true;
            return false;
        }
    }
}
