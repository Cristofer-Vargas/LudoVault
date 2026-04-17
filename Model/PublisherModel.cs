using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("publisher")]
    public class PublisherModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("name", TypeName = "varchar(60)")]
        [MaxLength(60)]
        public string Name { get; set; } = string.Empty;
        public List<GameModel> Games { get; set; } = [];     // Uma publisher pode ter vários (uma lista) de games
    }
}
