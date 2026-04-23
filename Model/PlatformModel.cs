using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("platform")]
    public class PlatformModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("name", TypeName = "varchar(20)")]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        public List<GamePlatformModel> GamePlatforms { get; set; } = [];
    }
}
