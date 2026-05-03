using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("user_library")]
    public class UserLibraryModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("user_id", TypeName = "int")]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public UserModel? User { get; set; }

        [Required]
        [Column("game_id", TypeName = "int")]
        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public GameModel? Game { get; set; }

        [Required]
        [Column("created_at", TypeName = "timestmap")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
