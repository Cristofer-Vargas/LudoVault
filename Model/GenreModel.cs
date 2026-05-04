using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
  [Table("genre")]
  public class GenreModel
  {
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(20)")]
    [MaxLength(20)]
    public string Name { get; set; } = string.Empty;

    public List<GameGenreModel> GameGenres { get; set; } = [];
  }
}
