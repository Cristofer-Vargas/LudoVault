namespace LudoVault.Configurations
{
  public static class CorsPolicyConfiguration
  {
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
      services.AddCors(options =>
      {
        options.AddPolicy("AllowReactApp",
                policy =>
                {
                  policy.WithOrigins("http://localhost:5173") // URL do seu React
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                });
      });

      return services;
    }

  }
}
