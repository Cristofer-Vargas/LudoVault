namespace LudoVault.Model
{
  public class GameGenreModel
  {
    public int Id { get; set; }
    public int GameId { get; set; }
    public int GenreId { get; set; }
    public GameModel? Game { get; set; }
    public GenreModel? Genre { get; set; }
  }
}
