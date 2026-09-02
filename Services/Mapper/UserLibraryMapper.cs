using LudoVault.DTO.Responses;
using LudoVault.Model;

namespace LudoVault.Services.Mapper
{
  public static class UserLibraryMapper
  {
    public static UserLibraryModel ToModel(int userId, int gameId)
    {
      return new UserLibraryModel
      {
        UserId = userId,
        GameId = gameId
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
        AddedAt = userLibrary.AddedAt.ToString()
      };
    }
  }
}
