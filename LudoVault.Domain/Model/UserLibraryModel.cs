namespace LudoVault.Domain.Model
{
  public class UserLibraryModel
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserModel? User { get; set; }
    public int GameId { get; set; }
    public GameModel? Game { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.Now;
  }
}
