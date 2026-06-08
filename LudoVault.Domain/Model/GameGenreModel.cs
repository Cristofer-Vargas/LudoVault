namespace LudoVault.Domain.Model
{
  public class GameGenreModel : EntityBaseModel
  {
    public int GameId { get; set; }
    public int GenreId { get; set; }
    public GameModel? Game { get; set; }
    public GenreModel? Genre { get; set; }
  }
}
