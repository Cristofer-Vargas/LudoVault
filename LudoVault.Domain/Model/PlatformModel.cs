namespace LudoVault.Domain.Model
{
  public class PlatformModel : EntityBaseModel
  {
    public string Name { get; set; } = string.Empty;
    public List<GamePlatformModel> GamePlatforms { get; set; } = [];
  }
}
