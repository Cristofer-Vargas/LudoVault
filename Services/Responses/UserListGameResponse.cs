namespace LudoVault.Services.Responses;

public class UserListGameResponse
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public string? Name { get; set; }
    public string? ImageUrl { get; set; }
    public string? PublisherName { get; set; }
    public string? CreatedAt { get; set; }
}