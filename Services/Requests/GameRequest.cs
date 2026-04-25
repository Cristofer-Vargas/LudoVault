using System.Text.Json.Serialization;

namespace LudoVault.Services.Requests
{
    public class GameRequest
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = "/caminhoImagem.jpg";
        public string? Description { get; set; }
        public long PublisherId { get; set; }

        [JsonPropertyName("platforms")]
        public List<long> PlatformIds { get; set; } = [];

        [JsonPropertyName("genres")]
        public List<long> GenreIds { get; set; } = [];
    }
}
