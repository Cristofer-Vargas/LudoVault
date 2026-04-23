using LudoVault.Configurations;
using LudoVault.Repositories;
using LudoVault.Repositories.Interfaces;
using LudoVault.Services;
using LudoVault.Services.Interfaces;

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

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IGameServices, GameServices>();
builder.Services.AddScoped<IPublisherServices, PublisherServices>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();

builder.Services.AddDatabaseConfiguration(builder.Configuration);
var app = builder.Build();
app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
