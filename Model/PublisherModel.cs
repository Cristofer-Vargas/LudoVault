using System.ComponentModel.DataAnnotations.Schema;

namespace LudoVault.Model
{
    [Table("publisher.publisher")]
    public class PublisherModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<GameModel> Games { get; set; } = new List<GameModel>();     // Uma publisher pode ter vários (uma lista) de games
    }
}
