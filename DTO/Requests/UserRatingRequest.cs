namespace LudoVault.DTO.Requests
{
  public class UserRatingRequest
  {
    public decimal Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
  }

}
