using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Authorization;
using BookProducerService.Infrastructure.Database;

namespace BookProducerService.APITest.Fixtures
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        //  public Status StatusFixture { get; set; }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
                // Add ApplicationDbContext using an in-memory database for testing.
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForAPITesting");
                    options.UseInternalServiceProvider(serviceProvider);
                });
                //services.AddMvc(options =>
                //{
                //    options.Filters.Add(new AllowAnonymousFilter());
                //    options.Filters.Add(new FakeUserFilter());
                //})
                // .AddApplicationPart(typeof(Startup).Assembly);
                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<ApplicationDbContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                // Ensure the database is created.
                context.Database.EnsureCreated();

                try
                {
                    new BookProducer.APITest.Helpers.DataSeeder().Populate(context);// Seed the database with test data.
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                        "database with test messages. Error: {Message}", ex.Message);
                }
            });
            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddJsonFile("appsettings.Test.json");

                //configBuilder.AddInMemoryCollection(
                //    new Dictionary<string, string>
                //    {
                //        ["ConnectionStrings:Practice1ServiceBusConnection"] = "Endpoint=sb://hdevelopment.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=6WA2C1f3tkmVpgSAFVLX8KVMI+0x+B9x+ZUHrzP5dJs="
                //    });
            });
        }
    }
}
