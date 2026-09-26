namespace LudoVault.DTO.Responses
{
  public class GameResponse
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<PublisherResponse> Publishers { get; set; } = [];
    public List<DeveloperResponse> Developers { get; set; } = [];
    public List<PlatformResponse> Platforms { get; set; } = [];
    public List<GenreResponse> Genres { get; set; } = [];
  }
}
