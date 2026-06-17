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
                  policy.WithOrigins("http://127.0.0.1:5500") // URL do seu APP
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                });
      });

      return services;
    }

  }
}
