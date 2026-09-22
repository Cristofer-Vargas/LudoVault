namespace LudoVault.Model
{
  public class PublisherGameModel : EntityBaseModel
  {
    public int PublisherId { get; set; }
    public int GameId { get; set; }
    public PublisherModel? Publisher { get; set; } 
    public GameModel? Game { get; set; }
  }
}