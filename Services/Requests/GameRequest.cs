using LudoVault.Model;

namespace LudoVault.Services.Requests
{
    public class GameRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Image_url { get; set; } = "/caminhoImagem.jpg";
        public string Description { get; set; } = string.Empty;
        public long PublisherId { get; set; }
    }
}
