namespace LudoVault.Domain.Model
{
  public class UserModel : EntityBaseModel
  {
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = "";
    public string Bio { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<RatingModel> Ratings { get; set; } = [];
    public List<UserListModel> Lists { get; set; } = [];
    public List<UserLibraryModel> UserLibraries { get; set; } = [];
  }
}
