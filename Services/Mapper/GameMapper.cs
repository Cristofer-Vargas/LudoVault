using LudoVault.Model;
using LudoVault.Services.Requests;
using LudoVault.Services.Responses;

namespace LudoVault.Services.Mapper
{
    public static class GameMapper
    {
        public static GameModel ToModel(GameRequest gameRequest, PublisherModel publisherModel)
        {
            return new GameModel()
            {
                Name = gameRequest.Name,
                Image_url = gameRequest.Image_url,
                Description = gameRequest.Description,
                PublisherId = gameRequest.PublisherId,
                Publisher = publisherModel,
                GamePlatforms = []
            };
        }

        public static GameResponse ToResponse(GameModel game)
        {
            return new GameResponse()
            {
                Id = game.Id.ToString(),
                Name = game.Name,
                Image_url = game.Image_url,
                Description = game.Description ?? "",
                PublisherName = game.Publisher.Name,
                Platforms = game.GamePlatforms
                    .Select(gp => new PlatformResponse
                    {
                        Id = gp.Platform.Id.ToString(),
                        Name = gp.Platform.Name
                    })
                    .ToList()
            };
        }
    }
}
