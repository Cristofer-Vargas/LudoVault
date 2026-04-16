using LudoVault.Model;

namespace LudoVault.Services.Responses
{
    public class GameResponse
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image_url { get; set; } = "/caminhoImagem.jpg";
        public string Description { get; set; } = string.Empty;
        public string PublisherName { get; set; } = string.Empty;
    }
}
