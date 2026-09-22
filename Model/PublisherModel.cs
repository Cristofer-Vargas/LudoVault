namespace LudoVault.Model
{
  public class PublisherModel : EntityBaseModel
  {
    public string Name { get; set; } = string.Empty;
    public DateOnly FundationAt { get; set; }
    public List<PublisherGameModel> PublisherGame { get; set; } = [];
  }
}
