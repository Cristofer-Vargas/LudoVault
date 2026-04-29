using System.Text.Json.Serialization;

namespace LudoVault.Services.Requests
{
    public class GameRequest
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = "/caminhoImagem.jpg";
        public string? Description { get; set; }
        public int PublisherId { get; set; }

        [JsonPropertyName("platforms")]
        public List<int> PlatformIds { get; set; } = [];

        [JsonPropertyName("genres")]
        public List<int> GenreIds { get; set; } = [];
    }
}
