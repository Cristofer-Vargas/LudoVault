namespace LudoVault.Domain.Model
{
  public class UserLibraryModel : EntityBaseModel
  {
    public int UserId { get; set; }
    public UserModel? User { get; set; }
    public int GameId { get; set; }
    public GameModel? Game { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.Now;
  }
}
