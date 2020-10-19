using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{ThreadId}] {Message} {NewLine}{Exception}";
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithThreadId()
                .WriteTo.Console(outputTemplate: logTemplate)
                .WriteTo.File("logs/.log", rollingInterval: RollingInterval.Day, outputTemplate: logTemplate,
                    fileSizeLimitBytes: null)
                .CreateLogger();

            try
            {
                Log.Information("Starting Application...");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
