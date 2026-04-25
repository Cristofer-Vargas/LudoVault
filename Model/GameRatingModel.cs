using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("game_rating")]
    public class GameRatingModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("rating", TypeName = "decimal(2,1)")]
        public decimal Rating { get; set; }

        [Required]
        [Column("game_id", TypeName = "int")]
        public long GameId { get; set; }
        public required GameModel Game { get; set; }

        [Required]
        [Column("user_id", TypeName = "int")]
        public long UserId { get; set; }
        public required UserModel User { get; set; }

        [Column("comment", TypeName = "text")]
        [MaxLength(255)]
        public string Comment { get; set; } = string.Empty;

        [Required]
        [Column("created_at", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
