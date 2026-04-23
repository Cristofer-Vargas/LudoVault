using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("game_platform")]
    public class GamePlatformModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("game_id", TypeName = "int")]
        public long GameId { get; set; }
        public required GameModel Game { get; set; }

        [Required]
        [Column("platform_id", TypeName = "int")]
        public long PlatformId { get; set; }
        public required PlatformModel Platform { get; set; }
    }
}
