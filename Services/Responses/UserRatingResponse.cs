namespace LudoVault.Services.Responses
{
    public class UserRatingResponse
    {
        public string? Id { get; set; }
        public decimal Rating { get; set; }
        public string? Comment { get; set; }
        public GameInfoResponse? Game { get; set; }   // GameResponse reduzido apenas para o contexto de Avaliações
        public string? CreatedAt { get; set; }
    }
}
