namespace LudoVault.Services.Responses
{
    public class GameRatingResponse
    {
        public string? Id { get; set; }
        public UserInfoResponse? User { get; set; }
        public decimal Rating { get; set; }
        public string? Comment { get; set; }
        public string? CreatedAt { get; set; }
    }
}
