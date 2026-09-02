namespace LudoVault.DTO.Responses
{
  public class RatingListUsersResponse
  {
    public List<RatingUserResponse> UsersRatings { get; set; } = [];
    public double? AvgRatings { get; set; }
    public int? TotalRatings { get; set; }
  }
}
