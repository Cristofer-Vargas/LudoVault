namespace LudoVault.Model
{
  public class DeveloperGameModel : EntityBaseModel
  {
    public int DeveloperId { get; set; }
    public int GameId { get; set; }
    public DeveloperModel? Developer { get; set; }
    public GameModel? Game { get; set; }
  }
}