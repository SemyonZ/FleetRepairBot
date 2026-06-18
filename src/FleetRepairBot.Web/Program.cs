using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FleetRepairBot.Telegram;
using FleetRepairBot.Infrastructure;
using FleetRepairBot.Data;
using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Data.Repositories;
using FleetRepairBot.Services;
using Telegram.Bot;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((ctx, cfg) => { cfg.AddEnvironmentVariables(); })
    .ConfigureServices((ctx, services) =>
    {
        var configuration = ctx.Configuration;

        // DbContext
        services.AddDbContext<FleetRepairDbContext>(opt => opt.UseInMemoryDatabase("fleet"));

        // Repositories and services
        services.AddScoped<IRepairRequestRepository, RepairRequestRepository>();
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IRepairRequestService, RepairRequestService>();

        // File storage
        services.AddSingleton<IFileStorage>(sp => new FileSystemStorage("./files"));

        // Resolve Telegram token: prefer environment variable TELEGRAM_BOT_TOKEN, fallback to configuration TelegramBot:BotToken
        var envToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
        var configToken = configuration["TelegramBot:BotToken"];
        var token = !string.IsNullOrWhiteSpace(envToken) ? envToken : configToken;

        if (!string.IsNullOrWhiteSpace(token))
        {
            // Bind other Telegram options from configuration section if present, but ensure Token is set from resolved value
            var telegramOptions = new TelegramBotOptions();
            configuration.GetSection("TelegramBot").Bind(telegramOptions);
            telegramOptions.Token = token;

            services.AddSingleton(telegramOptions);

            // Register Telegram client (singleton) and related components only when a token is present
            services.AddSingleton<ITelegramBotClient>(sp => new TelegramBotClient(telegramOptions.Token));

            services.AddSingleton<TelegramUpdateHandler>();
            services.AddHostedService<BotHostedService>();
        }
        else
        {
            // No token provided: do not register bot-related services. Application will run without the bot.
        }
    });

await builder.RunConsoleAsync();
