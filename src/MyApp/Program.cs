using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using MyApp.Services.Telegram;
using MyApp.Data;
using MyApp.Repositories;
using Microsoft.Extensions.Logging;

namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    cfg.AddEnvironmentVariables();
                })
                .ConfigureServices((ctx, services) =>
                {
                    // Bind legacy Telegram section if present
                    services.Configure<TelegramBotOptions>(ctx.Configuration.GetSection("Telegram"));

                    // Resolve token: prefer environment variable TELEGRAM_BOT_TOKEN, then configuration keys TelegramBot:BotToken and Telegram:BotToken
                    var envToken = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN");
                    var configToken = ctx.Configuration["TelegramBot:BotToken"] ?? ctx.Configuration["Telegram:BotToken"];
                    var token = !string.IsNullOrWhiteSpace(envToken) ? envToken : (configToken ?? string.Empty);

                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        services.AddSingleton<ITelegramBotClient>(sp => new TelegramBotClient(token));

                        // Repositories and services
                        services.AddScoped<IUserRepository, UserRepository>();
                        services.AddScoped<ITelegramUpdateHandler, TelegramUpdateHandler>();

                        // Hosted service for Telegram will only be registered when a token is provided
                        services.AddHostedService<TelegramBotHostedService>();

                        Console.WriteLine("Telegram bot token found: bot services registered.");
                    }
                    else
                    {
                        // Do not register Telegram client/hosted service when token is missing.
                        // Application will continue to run without starting the bot.
                        Console.WriteLine("Telegram bot token is not set. Telegram bot will not start.");

                        // Still register repositories/data services as normal (bot disabled)
                        services.AddScoped<IUserRepository, UserRepository>();
                    }

                    // DbContext - using InMemory by default for now; can be switched to real provider via configuration
                    services.AddDbContext<MyAppDbContext>(options => options.UseInMemoryDatabase("MyAppDb"));

                    // FileStorage: a minimal implementation registered inline
                    services.AddSingleton<IFileStorage, LocalFileStorage>();

                    services.AddLogging(config => config.AddConsole());
                })
                .Build();

            host.Run();
        }

        // Minimal file storage abstraction (registered in DI)
        public interface IFileStorage
        {
            void EnsureDirectory(string path);
        }

        private class LocalFileStorage : IFileStorage
        {
            public void EnsureDirectory(string path)
            {
                if (string.IsNullOrWhiteSpace(path)) return;
                try
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                catch
                {
                    // ignore for minimal implementation
                }
            }
        }
    }
}
