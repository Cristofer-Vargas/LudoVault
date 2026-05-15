using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
  [Table("user")]
  public class UserModel
  {
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(40)")]
    [MaxLength(40)]
    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    [Required]
    [Column("email", TypeName = "varchar(100)")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("avatar_url", TypeName = "varchar(255")]
    public string AvatarUrl { get; set; } = "";

    [Column("bio", TypeName = "text")]
    public string? Bio { get; set; }

    [Required]
    [Column("passwordHash", TypeName = "varchar(255)")]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<RatingModel> GamesRating { get; set; } = [];
  }
}
