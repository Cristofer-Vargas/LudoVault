using LudoVault.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCorsPolicy();
builder.Services.AddServicesAndRepositories();
builder.Services.AddValidations();

builder.Services.AddDatabaseConfiguration(builder.Configuration);

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