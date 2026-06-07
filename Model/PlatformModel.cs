namespace LudoVault.Model
{
  public class PlatformModel
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<GamePlatformModel> GamePlatforms { get; set; } = [];
  }
}
