namespace LudoVault.Domain.Model
{
  public class GenreModel : EntityBaseModel
  {
    public string Name { get; set; } = string.Empty;
    public List<GameGenreModel> GameGenres { get; set; } = [];
  }
}
