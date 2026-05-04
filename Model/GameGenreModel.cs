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
    public int Id { get; set; }

    [Required]
    [Column("game_id", TypeName = "int")]
    public int GameId { get; set; }
    [ForeignKey("GameId")]
    public GameModel? Game { get; set; }

    [Required]
    [Column("genre_id", TypeName = "int")]
    public int GenreId { get; set; }
    [ForeignKey("GenreId")]
    public GenreModel? Genre { get; set; }
  }
}
