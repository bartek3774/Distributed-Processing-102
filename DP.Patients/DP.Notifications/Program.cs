using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DP.Notifications
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSerilog((context, loggerConfig) =>
                    {
                        loggerConfig.ReadFrom.Configuration(context.Configuration);

                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            loggerConfig.WriteTo.Console();
                        }

                        loggerConfig.WriteTo.File(Path.Combine("", "log_.txt"), rollingInterval: RollingInterval.Day);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
