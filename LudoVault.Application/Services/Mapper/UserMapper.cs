using LudoVault.Application.DTO.Requests;
using LudoVault.Application.DTO.Responses;
using LudoVault.Domain.Model;

namespace LudoVault.Application.Services.Mapper
{
  public static class UserMapper
  {
    public static UserModel ToModel(UserRequest userRequest, string passwordHash)
    {
      return new UserModel()
      {
        Name = userRequest.Name,
        Email = userRequest.Email.ToLower().Trim(),
        Bio = userRequest.Bio,
        PasswordHash = passwordHash,
        AvatarUrl = userRequest.AvatarUrl ?? "/caminho_avatar_padrao.jpg"
      };
    }
    public static UserResponse ToResponse(UserModel user)
    {
      return new UserResponse()
      {
        Id = user.Id,
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
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        AvatarUrl = user.AvatarUrl
      };
    }
  }
}
