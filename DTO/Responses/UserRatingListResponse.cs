namespace LudoVault.DTO.Responses
{
  public class UserRatingListGamesResponse
  {
    public List<UserRatingGameResponse> Ratings { get; set; } = [];
    public int TotalRatings { get; set; }
  }
}
