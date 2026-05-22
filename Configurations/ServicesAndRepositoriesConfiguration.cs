using LudoVault.Repositories;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services;
using LudoVault.Services.Interfaces;

namespace LudoVault.Configurations
{
  public static class ServicesAndRepositoriesConfiguration
  {
    public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
    {
      // Serviços
      services.AddScoped<ISecurityServices, SecurityServices>();
      services.AddScoped<IGameServices, GameServices>();
      services.AddScoped<IUserServices, UserServices>();
      services.AddScoped<IPublisherServices, PublisherServices>();
      services.AddScoped<IImageServices, ImageServices>();
      services.AddScoped<ISystemServices, SystemServices>();

      // Repositórios
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IGameRepository, GameRepository>();
      services.AddScoped<IPublisherRepository, PublisherRepository>();

      return services;
    }
  }
}
