using FluentValidation;
using LudoVault.Application.DTO.Requests;
using LudoVault.Application.Validations;

namespace LudoVault.Application.Configurations
{
  public static class ValidationsConfiguration
  {
    public static IServiceCollection AddValidations(this IServiceCollection services)
    {
      // Validações com Fluent Validation
      services.AddScoped<IValidator<GameRequest>, GameValidation>();
      services.AddScoped<IValidator<UserRequest>, UserValidation>();
      services.AddScoped<IValidator<UserRatingRequest>, UserRatingValidation>();
      services.AddScoped<IValidator<UserListRequest>, UserListValidation>();
      services.AddScoped<IValidator<PublisherRequest>, PublisherValidation>();

      return services;
    }
  }
}
