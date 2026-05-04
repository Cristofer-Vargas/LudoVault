using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
  [Table("game")]
  public class GameModel
  {
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(100)")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column("image_url", TypeName = "varchar(255)")]
    [MaxLength(255)]
    public string ImageUrl { get; set; } = "/caminhoImagem.jpg";

    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    [Required]
    [Column("publisher_id", TypeName = "int")]
    public int PublisherId { get; set; }

    [Required]
    [Column("created_at", TypeName = "timestamp")]
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("PublisherId")]
    public required PublisherModel Publisher { get; set; } // Trás model de Publisher como referência a GameModel 
    public List<GamePlatformModel> GamePlatforms { get; set; } = [];
    public List<GameGenreModel> GameGenres { get; set; } = [];
    public List<GameRatingModel> GameRatings { get; set; } = [];

  }
}
