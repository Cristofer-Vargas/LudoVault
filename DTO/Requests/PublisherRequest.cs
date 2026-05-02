using System.ComponentModel.DataAnnotations;

namespace LudoVault.DTO.Requests
{
    public class PublisherRequest
    {
        [Required]
        public string? Name { get; set; }
    }
}
