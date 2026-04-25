namespace LudoVault.Services.Responses
{
    public class GameResponse
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = "/caminhoImagem.jpg";
        public string Description { get; set; } = string.Empty;
        public string PublisherName { get; set; } = string.Empty;
        public List<PlatformResponse> Platforms { get; set; } = [];
        public List<GenreResponse> Genres { get; set; } = [];
    }
}
