using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LudoVault.DTO.Requests
{
  public class GameRequest
  {
    [Required]
    public string? Name { get; set; }
    public string ImageUrl { get; set; } = "";
    public string? Description { get; set; }
    [Required]
    public int PublisherId { get; set; }

    [JsonPropertyName("platforms")]
    public List<int> PlatformIds { get; set; } = [];

    [JsonPropertyName("genres")]
    public List<int> GenreIds { get; set; } = [];
  }
}
