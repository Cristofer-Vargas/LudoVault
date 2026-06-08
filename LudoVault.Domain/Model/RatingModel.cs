namespace LudoVault.Domain.Model
{
  public class RatingModel
  {
    public int Id { get; set; }
    public decimal Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int GameId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public GameModel? Game { get; set; }
    public UserModel? User { get; set; }
  }
}
