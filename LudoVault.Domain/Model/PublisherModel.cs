namespace LudoVault.Domain.Model
{
  public class PublisherModel
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<GameModel> Games { get; set; } = [];     // Uma publisher pode ter vários (uma lista) de games
  }
}
