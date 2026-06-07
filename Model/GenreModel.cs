namespace LudoVault.Model
{
  public class GenreModel
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<GameGenreModel> GameGenres { get; set; } = [];
  }
}
