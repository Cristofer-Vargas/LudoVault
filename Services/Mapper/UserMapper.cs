using LudoVault.Model;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Mapper
{
    public static class UserMapper
    {
        public static UserModel ToModel(UserRequest userRequest, string passwordHash)
        {
            return new UserModel()
            {
                Name = userRequest.Name,
                Email = userRequest.Email,
                Bio = userRequest.Bio,
                PasswordHash = passwordHash,
                AvatarUrl = userRequest.AvatarUrl ?? "/caminho_avatar_padrao.jpg"
            };
        }
        public static UserResponse ToResponse(UserModel user)
        {
            return new UserResponse()
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Bio = user.Bio ?? "",
                AvatarUrl = user.AvatarUrl
            };
        }

        // Info será referido como "informações de entidade"
        // Dados reduzidos da entidade principal apenas para "informação"
        public static UserInfoResponse ToInfoRespose(UserModel user)
        {
            return new UserInfoResponse()
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl
            };
        }
    }
}
