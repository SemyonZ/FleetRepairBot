using FleetRepairBot.Data;
using FleetRepairBot.Data.Repositories;
using FleetRepairBot.Infrastructure;
using FleetRepairBot.Services;
using FleetRepairBot.Telegram;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Use in-memory DB for simplicity; replace with real provider via configuration
        services.AddDbContext<FleetRepairDbContext>(opt => opt.UseInMemoryDatabase("FleetRepair"));

        services.AddScoped<IRepairRequestRepository, RepairRequestRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();

        services.AddScoped<IRepairRequestService, RepairRequestService>();

        var filesRoot = Path.Combine(Directory.GetCurrentDirectory(), "files");
        services.AddSingleton<IFileStorage>(sp => new FileSystemStorage(filesRoot));

        services.Configure<TelegramBotOptions>(context.Configuration.GetSection("Telegram"));
        // TelegramUpdateHandler depends on scoped services (IRepairRequestService), so register it as scoped.
        services.AddScoped<TelegramUpdateHandler>();
        // BotHostedService will create scopes when it needs to resolve the handler, so it's safe to register it as hosted service.
        services.AddHostedService<BotHostedService>();
    })
    .Build();

await host.RunAsync();
