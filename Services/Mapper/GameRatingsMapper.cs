using LudoVault.Model;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Mapper
{
    public static class GameRatingsMapper
    {
        public static GameRatingResponse ToGameResponse(GameRatingModel gameRating)
        {
            return new GameRatingResponse()
            {
                Id = gameRating.Id.ToString(),
                User = UserMapper.ToInfoRespose(gameRating.User),
                Rating = gameRating.Rating,
                Comment = gameRating.Comment,
                CreatedAt = gameRating.CreatedAt.ToString()
            };
        }

        public static UserRatingResponse ToUserResponse(GameRatingModel userRating)
        {
            return new UserRatingResponse()
            {
                Id = userRating.Id.ToString(),
                Rating = userRating.Rating,
                Comment = userRating.Comment,
                Game = GameMapper.ToInfoResponse(userRating.Game),
                CreatedAt = userRating.CreatedAt.ToString()
            };
        }
    }
}
