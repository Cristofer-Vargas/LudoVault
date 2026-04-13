using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("game.game")]
    public class GameModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image_url { get; set; } = "/caminhoImagem.jpg";
        public string Description { get; set; } = string.Empty;
        public long PublisherId { get; set; }
        public DateTime CreateAt { get; set; }

        public required PublisherModel Publisher { get; set; } // Trás model de Publisher como referência a GameModel 

    }
}
