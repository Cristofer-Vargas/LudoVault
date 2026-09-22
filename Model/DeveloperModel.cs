namespace LudoVault.Model
{
  public class DeveloperModel : EntityBaseModel
  {
    public string Name { get; set; } = string.Empty;
    public DateOnly FundationAt { get; set; }
    public List<DeveloperGameModel> DeveloperGames { get; set; } = [];
  }
}
