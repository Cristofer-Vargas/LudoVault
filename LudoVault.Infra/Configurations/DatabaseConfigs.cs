using LudoVault.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace LudoVault.Infra.Configurations
{
  public static class DatabaseConfigs
  {
    public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services)
    {
      var connectionString = System.Environment.GetEnvironmentVariable("MySqlConnection");
      if (string.IsNullOrWhiteSpace(connectionString))
      {
        throw new ArgumentException("String de conexão 'MySqlConnection' não encontrada.");
      }

      services.AddDbContext<MysqlContext>
              (options => options.UseMySql
              (
                      connectionString,
                      Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(connectionString)
              )
              );
      return services;
    }
  }
}
