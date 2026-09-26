namespace LudoVault.Repositories.Base
{
  public static class ListUtils
  {
    public static List<T> AusenteEm<T>(this List<T> origem, List<T> destino)
    {
      return origem.Except(destino).ToList();
    }
  }
}