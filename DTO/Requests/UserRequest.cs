namespace LudoVault.DTO.Requests
{
  public class UserRequest
  {
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string Bio { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public string AvatarUrl { get; set; } = "";
  }
}
