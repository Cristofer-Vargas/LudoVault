namespace LudoVault.DTO.Responses
{
    public class GameRatingUserResponse
	{
        public int Id { get; set; }
        public string? Name { get; set; }
		public string? Email { get; set; }
		public string? AvatarUrl { get; set; }
		public decimal Rating { get; set; }
        public string? Comment { get; set; }
        public string? CreatedAt { get; set; }
    }
}
