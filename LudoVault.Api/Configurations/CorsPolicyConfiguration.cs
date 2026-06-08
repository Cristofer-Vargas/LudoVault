namespace LudoVault.Api.Configurations
{
  public static class CorsPolicyConfiguration
  {
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy("AllowApp",
                policy =>
                {
                  policy.WithOrigins("http://localhost:4200") // URL do seu APP
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                });
      });

      return services;
    }

  }
}
