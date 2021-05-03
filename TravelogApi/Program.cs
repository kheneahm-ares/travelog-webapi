using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;

namespace TravelogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var build = CreateHostBuilder(args).Build();

            using (var scope = build.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<AppDbContext>();

                    //apply any pending migrations
                    context.Database.Migrate();

                    Seed.SeedData(context).GetAwaiter().GetResult();
                }
                catch(Exception exc)
                {
                    //log here
                }

            }

            build.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
