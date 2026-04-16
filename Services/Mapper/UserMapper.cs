using LudoVault.Model;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Mapper
{
    public static class UserMapper
    {
        public static UserModel ToModel(UserRequest clienteRequest, string passwordHash)
        {
            return new UserModel()
            {
                Name = clienteRequest.Name,
                Email = clienteRequest.Email,
                Bio = clienteRequest.Bio,
                PasswordHash = passwordHash
            };
        }
        public static UserResponse ToResponse(UserModel user)
        {
            return new UserResponse()
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Bio = user.Bio ?? ""
            };
        }
    }
}
