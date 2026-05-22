namespace LudoVault.Services.Validations.Base
{
  public class Response
  {
    public List<Report> Report { get; }

    public Response()
    {
      Report = new List<Report>();
    }

    public Response(List<Report> reports)
    {
      Report = reports;
    }

    public Response(Report report) : this(new List<Report>() { report }) { }

    public static Response<T> Ok<T>(T data) => new Response<T>(data);
    public static Response Ok() => new Response();
    public static Response Unprocessable(List<Report> reports) => new Response(reports);
    public static Response Unprocessable(Report report) => new Response(report);
    public static Response<T> Unprocessable<T>(List<Report> reports)
    {
      return new Response<T>(reports);
    }
  }

  public class Response<T> : Response
  {
    public T? Data { get; set; }

    public Response()
    {

    }
    public Response(List<Report> reports) : base(reports)
    {
    }

    public Response(T data, List<Report>? reports = null) : base(reports ?? new List<Report>())
    {
      Data = data;
    }
  }
}
