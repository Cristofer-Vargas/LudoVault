using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("game_genre")]
    public class GameGenreModel
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
        [Column("genre_id", TypeName = "int")]
        public long GenreId { get; set; }
        public required GenreModel Genre { get; set; }
    }
}
