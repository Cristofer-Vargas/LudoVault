namespace LudoVault.DTO.Responses
{
  public class GameRatingListUsersResponse
  {
    public List<GameRatingUserResponse> UsersRatings { get; set; } = [];
    public double? AvgRatings { get; set; }
    public int? TotalRatings { get; set; }
  }
}
