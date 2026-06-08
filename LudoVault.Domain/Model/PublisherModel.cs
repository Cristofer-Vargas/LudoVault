namespace LudoVault.Domain.Model
{
  public class PublisherModel : EntityBaseModel
  {
    public string Name { get; set; } = string.Empty;
    public DateTime FundationAt { get; set; } = DateTime.Now;
    public List<GameModel> Games { get; set; } = [];     // Uma publisher pode ter vários (uma lista) de games
  }
}
