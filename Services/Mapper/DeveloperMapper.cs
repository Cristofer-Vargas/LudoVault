using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Model;

namespace LudoVault.Services.Mapper
{
  public static class DeveloperMap
  {
    public static DeveloperModel ToModel(DeveloperRequest developerRequest)
    {
      return new DeveloperModel()
      {
        Name = developerRequest.Name
      };
    }

    public static DeveloperResponse ToResponse(DeveloperModel developerModel, List<GameResponse> gamesResponse)
    {
      return new DeveloperResponse()
      {
        Id = developerModel.Id,
        Name = developerModel.Name,
        Games = gamesResponse
      };
    }
  }
}