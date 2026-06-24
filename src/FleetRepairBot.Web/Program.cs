using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FleetRepairBot.Data; // assuming concrete DbContext type is in this namespace

namespace FleetRepairBot.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                    // Fixed: register the concrete DbContext type instead of abstract DbContext
                    services.AddDbContext<FleetRepairDbContext>(options =>
                        options.UseSqlServer(connectionString));

                    // other registrations remain unchanged
                })
                .Build();

            host.Run();
        }
    }
}
