namespace LudoVault.DTO.Responses
{
    public class PublisherResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<GameResponse> Games { get; set; } = [];
    }
}
