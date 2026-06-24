using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Data;
using FleetRepairBot.Data.Repositories;
using FleetRepairBot.Services;
using FleetRepairBot.Infrastructure;
using FleetRepairBot.Telegram;

var builder = WebApplication.CreateBuilder(args);

// Configuration & Services
var configuration = builder.Configuration;
var services = builder.Services;

// Controllers
services.AddControllers().AddJsonOptions(opts => { });

// Database: allow switching between InMemory (dev) and SQL Server via config
var connectionString = configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("(localdb)") && builder.Environment.IsDevelopment())
{
    services.AddDbContext<FleetRepairDbContext>(options =>
        options.UseInMemoryDatabase("FleetRepairBotDb"));
}
else
{
    services.AddDbContext<FleetRepairDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// Repositories
services.AddScoped<IRepairRequestRepository, RepairRequestRepository>();
services.AddScoped<IDriverRepository, DriverRepository>();
services.AddScoped<IVehicleRepository, VehicleRepository>();

// Services
services.AddScoped<IRepairRequestService, RepairRequestService>();

// File storage registration - use configured base path or default to "uploads" under content root
var basePath = configuration.GetValue<string>("FileStorage:BasePath");
if (string.IsNullOrWhiteSpace(basePath))
{
    basePath = Path.Combine(AppContext.BaseDirectory, "uploads");
}
services.AddSingleton<IFileStorage>(sp => new FileSystemStorage(basePath));

// Telegram options
var telegramOptions = configuration.GetSection("TelegramBot").Get<TelegramBotOptions>() ?? new TelegramBotOptions();
var envToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
if (!string.IsNullOrWhiteSpace(envToken)) telegramOptions.BotToken = envToken;
services.AddSingleton(telegramOptions);

// Telegram hosted service and handler (existing implementation in FleetRepairBot.Telegram project)
services.AddSingleton<TelegramUpdateHandler>();
services.AddHostedService<BotHostedService>();

var app = builder.Build();

// Minimal middleware
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Ensure DB created in development and seed minimal data
using (var scope = app.Services.CreateScope())
{
    var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
    var db = scope.ServiceProvider.GetRequiredService<FleetRepairDbContext>();
    if (env.IsDevelopment())
    {
        try
        {
            db.Database.EnsureCreated();
        }
        catch
        {
            // best-effort
        }
    }
}

app.Run();
