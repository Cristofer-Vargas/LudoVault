using LudoVault.Configurations;
using LudoVault.Repositories;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services;
using LudoVault.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<ISecurityService, SecurityService>();

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IGameServices, GameServices>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();

builder.Services.AddDatabaseConfiguration(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
