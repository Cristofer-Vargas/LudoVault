using LudoVault.Model;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services.Interfaces;
using LudoVault.Services.Mapper;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

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

        public async Task<UserResponse> BuscarUsuarioPorId(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponse> CriarUsuario(UserRequest user)
        {
            user.PasswordHash = await _securityService.EncryptPassword(user.PasswordHash);
            var userModel = UserMapper.ToModel(user, user.PasswordHash);

            UserResponse userResponse = UserMapper.ToResponse(await _userRepository.CriarUsuario(userModel));
            return userResponse;
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
            throw new NotImplementedException();
        }
    }
}
