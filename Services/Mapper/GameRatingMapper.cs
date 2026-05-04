using LudoVault.DTO.Responses;
using LudoVault.Model;

namespace LudoVault.Services.Mapper
{
  public static class GameRatingMapper
  {
    public static GameRatingUserResponse ToGameUserResponse(GameRatingModel userRating)
    {
      return new GameRatingUserResponse()
      {
        Id = userRating.Id,
        Name = userRating.User.Name,
        Email = userRating.User.Email,
        AvatarUrl = userRating.User.AvatarUrl,
        Rating = userRating.Rating,
        Comment = userRating.Comment,
        CreatedAt = userRating.CreatedAt.ToString()
      };
    }

    public static UserRatingGameResponse ToUserGameResponse(GameRatingModel userRating)
    {
      return new UserRatingGameResponse
      {
        Id = userRating.Id,
        Name = userRating.Game.Name,
        ImageUrl = userRating.Game.ImageUrl,
        Rating = userRating.Rating,
        Comment = userRating.Comment,
        CreatedAt = userRating.CreatedAt.ToString()
      };
    }
  }
}
