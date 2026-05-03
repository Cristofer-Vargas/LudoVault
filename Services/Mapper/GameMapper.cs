using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Model;

namespace LudoVault.Services.Mapper
{
    public static class GameMapper
    {
        public static GameModel ToModel(GameRequest gameRequest, PublisherModel publisherModel, List<int> platformIds, List<int> genreIds)
        {
            return new GameModel()
            {
                Name = gameRequest.Name,
                ImageUrl = gameRequest.ImageUrl,
                Description = gameRequest.Description,
                PublisherId = gameRequest.PublisherId,
                Publisher = publisherModel,
                GamePlatforms = platformIds
                    .Select(id => new GamePlatformModel { PlatformId = id })
                    .ToList(),
                GameGenres = genreIds
                    .Select(id => new GameGenreModel { GenreId = id })
                    .ToList()
            };
        }

        public static GameResponse ToResponse(GameModel game)
        {
            return new GameResponse()
            {
                Id = game.Id,
                Name = game.Name,
                ImageUrl = game.ImageUrl,
                Description = game.Description ?? "",
                PublisherName = game.Publisher.Name,
                Platforms = game.GamePlatforms
                    .Where(gp => gp.Platform != null)
                    .Select(gp => new PlatformResponse
                    {
                        Id = gp.Platform.Id,
                        Name = gp.Platform.Name
                    })
                    .ToList() ?? [],
                Genres = game.GameGenres
                    .Where(gg => gg.Genre != null)
                    .Select(gg => new GenreResponse
                    {
                        Id = gg.Genre.Id,
                        Name = gg.Genre.Name
                    })
                    .ToList() ?? []
            };
        }

        public static GameInfoResponse ToInfoResponse(GameModel game)
        {
            return new GameInfoResponse()
            {
                Id = game.Id,
                Name = game.Name,
                ImageUrl = game.ImageUrl
            };
        }
    }
}
