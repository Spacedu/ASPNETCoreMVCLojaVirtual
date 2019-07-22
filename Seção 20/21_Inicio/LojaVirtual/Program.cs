using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using LojaVirtual.Libraries.Bug;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace LojaVirtual
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CurrentDirectoryHelpers.SetCurrentDirectory();
            string caminhoLog = Path.Combine(Directory.GetCurrentDirectory(), "Logs.txt");

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.File(caminhoLog)
            .CreateLogger();

            try
            {
                Log.Information("--- SERVIDOR INICIANDO ---");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "--- SERVIDOR TERMINOU INESPERADAMENTE ---");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
