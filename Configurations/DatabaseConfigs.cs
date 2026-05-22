using LudoVault.Data;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Configurations
{
  public static class DatabaseConfigs
  {
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
      var connectionString = configuration["MySqlConnection"];
      if (string.IsNullOrWhiteSpace(connectionString))
      {
        throw new ArgumentException("String de conexão 'MySqlConnection' não encontrada.");
      }

      services.AddDbContext<MysqlContext>
              (options => options.UseMySql
              (
                      connectionString,
                      ServerVersion.AutoDetect(connectionString)
              )
              );
      return services;
    }
  }
}
