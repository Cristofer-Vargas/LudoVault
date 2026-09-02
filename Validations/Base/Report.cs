namespace LudoVault.Validations.Base
{
  public class Report(string message, int? code = null)
  {
    public int? Code { get; set; } = code;
    public string? Message { get; set; } = message;

    public static Report Create(string message, int? code = null) => new Report(message, code);
  }
}
