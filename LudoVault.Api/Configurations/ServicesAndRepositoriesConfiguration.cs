using LudoVault.Domain.Interfaces.Repositories;
using LudoVault.Application.Interfaces.Services;
using LudoVault.Application.Services;
using LudoVault.Infra.Services;
using LudoVault.Infra.Repositories;

namespace LudoVault.Api.Configurations
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
      services.AddScoped<IPlatformServices, PlatformServices>();
      services.AddScoped<IGenreServices, GenreServices>();

      // Repositórios
      services.AddScoped<IUserRepository, UserRepository>();
      services.AddScoped<IGameRepository, GameRepository>();
      services.AddScoped<IPlatformRepository, PlatformRepository>();
      services.AddScoped<IGenreRepository, GenreRepository>();
      services.AddScoped<IPublisherRepository, PublisherRepository>();

      return services;
    }
  }
}
