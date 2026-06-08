namespace LudoVault.Domain.Model
{
  public class UserListModel : EntityBaseModel
  {
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public UserModel? User { get; set; }
    public List<UserListItemModel> ListItems { get; set; } = [];
  }
}
