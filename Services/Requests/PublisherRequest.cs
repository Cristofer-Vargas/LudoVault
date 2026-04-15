namespace LudoVault.Services.Requests
{
    public class PublisherRequest
    {
        public string Name { get; set; } = string.Empty;
        public List<GameRequest> Games { get; set; } = new List<GameRequest>();
    }
}
