namespace LudoVault.Services.Responses
{
    public class UserRatingListGamesResponse
    {
        public List<UserRatingGameResponse> GamesRatings { get; set; } = [];
        public int? TotalRatings { get; set; }
    }
}
