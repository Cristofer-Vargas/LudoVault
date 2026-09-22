namespace LudoVault.DTO.Responses
{
  public class DeveloperResponse
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<GameResponse> Games { get; set; } = [];
  }
}