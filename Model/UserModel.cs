namespace LudoVault.Model
{
  public class UserModel
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = "";
    public string? Bio { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<RatingModel> Ratings { get; set; } = [];
    public List<UserListModel> Lists { get; set; } = [];
    public List<UserLibraryModel> UserLibraries { get; set; } = [];
  }
}
