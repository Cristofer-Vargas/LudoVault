using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
  [Table("user_list")]
  public class UserListModel
  {
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(60)")]
    [MaxLength(60)]
    public string? Name { get; set; }

    [Required]
    [Column("user_id", TypeName = "int")]
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public UserModel? User { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<UserListGameModel> ListItems { get; set; } = [];
  }
}
