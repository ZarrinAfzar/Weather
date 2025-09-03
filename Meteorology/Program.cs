using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Weather.Data.DBInitializer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Weather
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    Initializer.CreateRoles(services).Wait();
                }
                catch (Exception ex)
                {
                    //var logger = services.GetRequiredService<Logger<Program>>();
                    //logger.LogError(ex, "خطا در داده های اولیه");
                }
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
     WebHost.CreateDefaultBuilder(args)
         .UseStartup<Startup>()
         .CaptureStartupErrors(true)
         .UseSetting("detailedErrors", "true")
         .Build();

    }
}
