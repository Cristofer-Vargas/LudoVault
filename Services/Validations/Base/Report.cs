namespace LudoVault.Services.Validations.Base
{
  public class Report
  {
    public int? Code { get; set; }
    public string? Message { get; set; }

    public Report()
    {

    }

    public Report(string message, int? code = null)
    {
      Code = code;
      Message = message;
    }

    public static Report Create(string message, int? code = null) => new Report(message, code);
  }
}
