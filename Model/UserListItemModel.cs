namespace LudoVault.Model
{
  public class UserListItemModel
  {
    public int Id { get; set; }
    public int ListId { get; set; }
    public int GameId { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    public GameModel? Game { get; set; }
    public UserListModel? UserList { get; set; }
  }
}
