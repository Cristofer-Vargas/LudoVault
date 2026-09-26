using LudoVault.DTO.Requests;
using LudoVault.DTO.Responses;
using LudoVault.Model;
using LudoVault.Services.Mapper.Interfaces;

namespace LudoVault.Services.Mapper
{
  public static class GameMapper
  {
    public static GameModel ToModel(GameRequest gameRequest)
    {
      return new GameModel()
      {
        Name = gameRequest.Name,
        ImageUrl = gameRequest.ImageUrl,
        Description = gameRequest.Description,
        GamePlatforms = gameRequest.PlatformIds
                      .Select(id => new GamePlatformModel { PlatformId = id })
                      .ToList(),
        GameGenres = gameRequest.GenreIds
                      .Select(id => new GameGenreModel { GenreId = id })
                      .ToList(),
        GamePublishers = gameRequest.PublisherIds
                      .Select(id => new PublisherGameModel { PublisherId = id })
                      .ToList(),
        GameDevelopers = gameRequest.DeveloperIds
                      .Select(id => new DeveloperGameModel { DeveloperId = id })
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
        Description = game.Description,
        Platforms = game.GamePlatforms.Select(gp => new PlatformResponse
                      {
                        Id = gp.Platform.Id,
                        Name = gp.Platform.Name
                      })
                      .ToList() ?? [],
        Genres = game.GameGenres.Select(gg => new GenreResponse
                      {
                        Id = gg.Genre.Id,
                        Name = gg.Genre.Name
                      })
                      .ToList() ?? [],
        Publishers = game.GamePublishers.Select(gp => new PublisherResponse
                      {
                        Id = gp.Publisher.Id,
                        Name = gp.Publisher.Name
                      }).ToList() ?? [],
        Developers = game.GameDevelopers.Select(gd => new DeveloperResponse
                      {
                        Id = gd.Developer.Id,
                        Name = gd.Developer.Name
                      }).ToList() ?? []
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
