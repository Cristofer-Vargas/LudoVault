using System.ComponentModel.DataAnnotations;

namespace LudoVault.DTO.Requests
{
    public class UserRequest
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        public string Bio { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
    }
}
