namespace LudoVault.Domain.Model
{
  public class GamePlatformModel : EntityBaseModel
  {
    public int GameId { get; set; }
    public int PlatformId { get; set; }
    public PlatformModel? Platform { get; set; }
    public GameModel? Game { get; set; }
  }
}
