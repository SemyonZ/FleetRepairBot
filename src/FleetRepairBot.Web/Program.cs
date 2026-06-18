using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using FleetRepairBot.Data;
using FleetRepairBot.Data.Repositories;
using FleetRepairBot.Services;
using FleetRepairBot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext: use InMemory by default to simplify local development and CI. This avoids hard dependency on external DB for basic runs/tests.
builder.Services.AddDbContext<FleetRepairDbContext>(options =>
    options.UseInMemoryDatabase("FleetRepair"));

// Repositories and services registration (scoped lifetime for DB-scoped services)
// Note: namespaces and types align with repository/service folders
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IRepairRequestService, RepairRequestService>();
builder.Services.AddScoped<IFileStorage, FileSystemStorage>();

// Telegram client and hosted service
// Priority for token: environment variable TELEGRAM_BOT_TOKEN; fallback to configuration Telegram:BotToken
var envToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
var configToken = configuration["Telegram:BotToken"];
var botToken = !string.IsNullOrWhiteSpace(envToken) ? envToken : (configToken ?? string.Empty);

if (!string.IsNullOrWhiteSpace(botToken))
{
    // Register Telegram client only when a token is available
    builder.Services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(botToken));

    // Try to register BotHostedService by reflection to avoid compile-time dependency if the implementation is missing.
    // This ensures the application won't fail to start when the hosted-service class isn't present; the bot will simply not start.
    var hostedType = Type.GetType("FleetRepairBot.Telegram.BotHostedService, FleetRepairBot.Telegram");
    if (hostedType != null && typeof(IHostedService).IsAssignableFrom(hostedType))
    {
        // Register the hosted service using the non-generic overload via the IHostedService service type
        builder.Services.AddSingleton(typeof(IHostedService), provider =>
            ActivatorUtilities.CreateInstance(provider, hostedType));
    }
    else
    {
        // If BotHostedService implementation is not present, skip hosted service registration.
        // The bot client is still registered (useful for other parts), but no hosted background processing will run.
        Console.WriteLine("BotHostedService type not found; Telegram bot client registered but hosted service not started.");
    }
}
else
{
    // No token provided: do not register Telegram client or hosted service. Application must not crash on start.
    Console.WriteLine("No Telegram bot token configured (TELEGRAM_BOT_TOKEN env or Telegram:BotToken). Telegram bot will not be started.");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
