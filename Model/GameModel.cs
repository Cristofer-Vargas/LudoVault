using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("game.game")]
    public class GameModel
    {
        public int id { get; set; }

    }
}
