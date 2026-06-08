using Serilog;

namespace LudoVault.Api.Configurations
{
  public static class LogConfiguration
  {
    public static void AddLoggerSerilog(this WebApplicationBuilder builder)
    {
      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Debug()
        .CreateLogger();

      builder.Host.UseSerilog();
    }
  }
}
