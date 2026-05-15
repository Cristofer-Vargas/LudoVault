using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Model;

namespace LudoVault.Services.Mapper
{
  public static class RatingMapper
  {
    public static RatingModel ToRatingModel(UserRatingRequest userRating, UserModel user, GameModel game)
    {
      return new RatingModel
      {
        Rating = Math.Round(userRating.Rating, 1),
        User = user,
        UserId = user.Id,
        Game = game,
        GameId = game.Id,
        Comment = userRating.Comment
      };
    }

    public static RatingUserResponse ToGameUserResponse(RatingModel userRating)
    {
      return new RatingUserResponse()
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

    public static UserRatingGameResponse ToUserGameResponse(RatingModel userRating)
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
