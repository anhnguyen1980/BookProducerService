using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using BookProducerService.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
//using Serilog.Exception;

namespace BookProducerService
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            //Log.Logger = new LoggerConfiguration()
            //           .MinimumLevel.Debug()
            //           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //           .Enrich.FromLogContext()
            //           // .Enrich.WithExceptionDetails()
            //           .WriteTo.Console()
            //           .WriteTo.File(
            //               "Logs/log.txt",                           
            //                 shared: true,
            //                 flushToDiskInterval: TimeSpan.FromSeconds(5),
            //                 rollOnFileSizeLimit: true,
            //               rollingInterval: RollingInterval.Minute)//combine date into a log file name
            //           .CreateLogger();
            var host = CreateHostBuilder(args).Build();

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(host.Services.GetRequiredService<IConfiguration>())
            .Enrich.WithProperty("ApplicationName", "ProcuderService")
            .CreateLogger();

            // CreateHostBuilder(args).Build().Run();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Host created");
            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    if (context.Database.EnsureCreated())
                        context.Database.Migrate();
                    logger.LogInformation("Seeding the db");
                    new BookProducerService.Infrastructure.Database.DataSeeder().Populate(context);
                }
                logger.LogInformation("Starting web host");
                host.Run();
                logger.LogInformation("Stopping web host");
            }
            catch (Exception ex)
            {

                logger.LogError($"An error occurred in Main {ex.Message}");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
