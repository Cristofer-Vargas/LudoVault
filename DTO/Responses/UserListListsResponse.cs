namespace LudoVault.DTO.Responses
{
  public class UserListListsResponse
  {
    public int? Id { get; set; }
    public string? ListName { get; set; }
    public List<UserListGameResponse> Games { get; set; } = [];
    public int? TotalGames { get; set; }
  }
}
