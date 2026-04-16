namespace LudoVault.Services.Responses
{
    public class PublisherResponse
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<GameResponse> Games { get; set; } = [];
    }
}
