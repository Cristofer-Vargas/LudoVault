﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("game_platform")]
    public class GamePlatformModel
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
        [Column("platform_id", TypeName = "int")]
        public int PlatformId { get; set; }
        [ForeignKey("PlatformId")]
        public PlatformModel? Platform { get; set; }
    }
}
