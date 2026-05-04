using System.ComponentModel.DataAnnotations;

namespace LudoVault.DTO.Requests
{
  public class UserLibraryRequest
  {
    [Required]
    public int UserId { get; set; }
    [Required]
    public int GameId { get; set; }
  }
}
