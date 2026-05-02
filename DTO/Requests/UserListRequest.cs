using System.ComponentModel.DataAnnotations;

namespace LudoVault.DTO.Requests
{
    public class UserListRequest
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
