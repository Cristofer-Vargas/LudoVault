using LudoVault.Api.Configurations;
using LudoVault.Application.Configurations;
using LudoVault.Infra.Configurations;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.TraversePath().Load();

builder.Services.AddControllers();
builder.Services.AddCorsPolicy();
builder.Services.AddServicesAndRepositories();
builder.Services.AddValidations();

builder.Services.Configure<DefaultImagesOptions>(builder.Configuration.GetSection("ImageProvider:DefaultImages"));

builder.Services.AddDatabaseConfiguration();

// Log System
builder.AddLoggerSerilog();

// Configure the HTTP request pipeline.
var app = builder.Build();

app.UseCors("AllowApp");
app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();