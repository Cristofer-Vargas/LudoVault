namespace LudoVault.Domain.Model
{
  public class GameModel : EntityBaseModel
  {
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PublisherId { get; set; }
    public DateTime LauchedAt { get; set; } = DateTime.UtcNow;
    public PublisherModel? Publisher { get; set; }
    public List<GamePlatformModel> GamePlatforms { get; set; } = [];
    public List<GameGenreModel> GameGenres { get; set; } = [];
    public List<RatingModel> GameRatings { get; set; } = [];
    public List<UserListItemModel> ListItems { get; set; } = [];
    public List<UserLibraryModel> UserLibraries { get; set; } = [];
  }
}
