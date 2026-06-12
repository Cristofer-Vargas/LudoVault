namespace LudoVault.Domain.Model
{
  public class PublisherModel : EntityBaseModel
  {
    public string Name { get; set; } = string.Empty;
    public DateOnly FundationAt { get; set; }
    public List<GameModel> Games { get; set; } = [];     // Uma publisher pode ter vários (uma lista) de games
  }
}
