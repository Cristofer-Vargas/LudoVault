using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Model;

namespace LudoVault.Services.Mapper
{
  public static class UserLibraryMapper
  {
    public static UserLibraryModel ToModel(UserLibraryRequest userLibrary)
    {
      return new UserLibraryModel
      {
        UserId = userLibrary.UserId,
        GameId = userLibrary.GameId
      };
    }

    public static UserLibraryGameResponse ToGameResponse(UserLibraryModel userLibrary)
    {
      return new UserLibraryGameResponse
      {
        Id = userLibrary.Id,
        GameId = userLibrary.GameId,
        Name = userLibrary.Game.Name,
        ImageUrl = userLibrary.Game.ImageUrl,
        AddedAt = userLibrary.CreatedAt.ToString()
      };
    }
  }
}
