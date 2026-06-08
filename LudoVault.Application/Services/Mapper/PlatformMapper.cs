using LudoVault.Application.DTO.Responses;
using LudoVault.Domain.Model;

namespace LudoVault.Application.Services.Mapper
{
  public static class PlatformMapper
  {
    public static GamePlatformModel ToGamePlatformModel(int id)
    {
      return new GamePlatformModel()
      {
        PlatformId = id
      };
    }

    public static PlatformResponse ToResponse(PlatformModel platform)
    {
      return new PlatformResponse()
      {
        Id = platform.Id,
        Name = platform.Name
      };
    }
  }
}
