using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Telegram;
using FleetRepairBot.Data;

// Note: This Program.cs registers DB context (SQL Server), Telegram handler and hosted service.
// Some repository/service/file-storage registrations were removed because corresponding implementations
// are not present in the package. TODO: add implementations and re-register them.

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Configure options from appsettings
builder.Services.Configure<TelegramBotOptions>(configuration.GetSection("TelegramBot"));

// Register DbContext - SQL Server (no InMemory)
// Ensure the connection string exists in appsettings.json under ConnectionStrings:DefaultConnection
builder.Services.AddDbContext<FleetRepairDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// NOTE: The application originally attempted to register IRepairRequestRepository/IRepairRequestService and IFileStorage
// but the concrete implementations are not present in this package. To keep the project buildable and runnable,
// these registrations are temporarily omitted. When implementations are added, register them here.
// Example TODO:
// builder.Services.AddScoped<IRepairRequestRepository, RepairRequestRepository>();
// builder.Services.AddScoped<IRepairRequestService, RepairRequestService>();
// builder.Services.AddSingleton<IFileStorage, FileSystemStorage>();

// Telegram handler and hosted service
builder.Services.AddSingleton<TelegramUpdateHandler>();
builder.Services.AddHostedService<BotHostedService>();

// Add controllers or minimal API endpoints as needed
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => Results.Text("FleetRepairBot Web API is running."));

app.MapControllers();

app.Run();
