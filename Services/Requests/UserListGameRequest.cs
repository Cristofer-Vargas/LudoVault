using System.ComponentModel.DataAnnotations;

namespace LudoVault.Services.Requests
{
    public class UserListGameRequest
    {
        [Required]
        public int ListId { get; set; }
        [Required]
        public int GameId { get; set; }
    }
}
