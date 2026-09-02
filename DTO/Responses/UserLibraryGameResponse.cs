namespace LudoVault.DTO.Responses
{
  public class UserLibraryGameResponse
  {
    public int Id { get; set; }
    public int GameId { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? AddedAt { get; set; }
  }
}
