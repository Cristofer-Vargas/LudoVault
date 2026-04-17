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
            var userResponse = UserMapper.ToResponse(await _userRepository.BuscarUsuarioPorId(id));
            return userResponse;
        }

        public async Task<UserResponse> CriarUsuario(UserRequest user)
        {
            user.PasswordHash = await _securityService.EncryptPassword(user.PasswordHash);
            var userModel = UserMapper.ToModel(user, user.PasswordHash);

            UserResponse userResponse = UserMapper.ToResponse(await _userRepository.CriarUsuario(userModel));
            return userResponse;
        }

        public async Task<UserResponse> AtualizarUsuario(UserRequest user, long id)
        {
            var userExisted = await _userRepository.BuscarUsuarioPorId(id);

            userExisted.Name = user.Name;
            userExisted.Email = user.Email;
            userExisted.PasswordHash = await _securityService.EncryptPassword(user.PasswordHash);
            userExisted.Bio = user.Bio;

            UserResponse userRes =  UserMapper.ToResponse(await _userRepository.Atualizar(userExisted, id));
            return userRes;
        }

        public async Task<bool> VerificarEmailEmUso(string email)
        {
            bool emailJaExiste = await _userRepository.VerificarEmailExistente(email);
            if (emailJaExiste) return true;
            return false;
        }

    }
}
