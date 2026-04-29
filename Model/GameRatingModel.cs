﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("game_rating")]
    public class GameRatingModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("rating", TypeName = "decimal(2,1)")]
        public decimal Rating { get; set; }

        [Required]
        [Column("game_id", TypeName = "int")]
        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public required GameModel Game { get; set; }

        [Required]
        [Column("user_id", TypeName = "int")]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public required UserModel User { get; set; }

        [Column("comment", TypeName = "text")]
        [MaxLength(255)]
        public string Comment { get; set; } = string.Empty;

        [Required]
        [Column("created_at", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
