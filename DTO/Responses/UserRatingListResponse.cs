namespace LudoVault.DTO.Responses
{
    public class UserRatingListGamesResponse
    {
        public List<UserRatingGameResponse> GamesRatings { get; set; } = [];
        public int? TotalRatings { get; set; }
    }
}
