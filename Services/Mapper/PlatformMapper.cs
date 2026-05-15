using LudoVault.DTO.Responses;
using LudoVault.Model;

namespace LudoVault.Services.Mapper
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
