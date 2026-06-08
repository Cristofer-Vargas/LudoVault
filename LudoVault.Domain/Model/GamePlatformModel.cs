namespace LudoVault.Domain.Model
{
  public class GamePlatformModel
  {
    public int Id { get; set; }
    public int GameId { get; set; }
    public int PlatformId { get; set; }
    public PlatformModel? Platform { get; set; }
    public GameModel? Game { get; set; }
  }
}
