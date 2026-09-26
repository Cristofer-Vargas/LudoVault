using System.Text.Json.Serialization;

namespace LudoVault.DTO.Requests
{
  public class GameRequest
  {
    public string? Name { get; set; }
    public string ImageUrl { get; set; } = "";
    public string? Description { get; set; }

    [JsonPropertyName("platforms")]
    public HashSet<int> PlatformIds { get; set; } = [];

    [JsonPropertyName("genres")]
    public HashSet<int> GenreIds { get; set; } = [];

    [JsonPropertyName("publishers")]
    public HashSet<int> PublisherIds { get; set; } = [];

    [JsonPropertyName("developers")]
    public HashSet<int> DeveloperIds { get; set; } = [];
  }
}
