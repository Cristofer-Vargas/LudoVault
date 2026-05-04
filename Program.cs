using LudoVault.Configurations;
using LudoVault.Repositories;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services;
using LudoVault.Services.Interfaces;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowReactApp",
          policy =>
          {
            policy.WithOrigins("http://localhost:5173") // URL do seu React
                                .AllowAnyHeader()
                                .AllowAnyMethod();
          });
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ISecurityService, SecurityService>();

// Serviços
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IGameServices, GameServices>();
builder.Services.AddScoped<IPublisherServices, PublisherServices>();
builder.Services.AddScoped<IImageServices, ImageServices>();
builder.Services.AddScoped<ISystemServices, SystemServices>();

// Repositórios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();

builder.Services.AddDatabaseConfiguration(builder.Configuration);
var app = builder.Build();
app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
