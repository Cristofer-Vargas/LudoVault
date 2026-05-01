using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("user_list_items")]
    public class UserListGameModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("list_id")]
        public int ListId { get; set; }
        [ForeignKey("ListId")]
        public UserListModel? UserList { get; set; }

        [Required]
        [Column("game_id")]
        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public GameModel? Game { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
