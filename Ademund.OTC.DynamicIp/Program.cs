using Ademund.OTC.DynamicIp.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.IO;

namespace Ademund.OTC.DynamicIp
{
    // dotnet user-secrets init
    // dotnet user-secrets set "DynamicIp:AccessKey" "ak"
    // dotnet user-secrets set "DynamicIp:SecretKey" "sk"
    // dotnet user-secrets set "DynamicIp:ProjectId" "pid"
    // dotnet user-secrets set "DynamicIp:SecurityGroupId" "sgid"

    internal class Program
    {
        private static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                Console.Title = "Ademund.OTC.DynamicIp";
            }
            Directory.SetCurrentDirectory(Path.GetDirectoryName(AppContext.BaseDirectory));
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddUserSecrets<Program>();
                    configApp.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    configApp.AddEnvironmentVariables();
                    configApp.AddCommandLine(args);
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();
                    services.AddSingleton(hostContext.Configuration.GetSection("DynamicIp").Get<DynamicIpConfig>());
                    services.AddSingleton<CurrentIP>();
                    services.AddSingleton<ISystrayMenu, SystrayMenu>();
                    services.AddSingleton<IIPChecker, IPChecker>();
                    services.AddHostedService<IPCheckerService>();
                    services.AddSingleton<ILoggerProvider>(_ =>
                    {
                        var logger = new LoggerConfiguration()
                            .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "Logs", "log.txt"),
                                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug,
                                rollingInterval: RollingInterval.Day,
                                fileSizeLimitBytes: 1000000,
                                retainedFileCountLimit: 7
                                )
                            .CreateLogger();
                        return new SerilogLoggerProvider(logger, dispose: true);
                    });
                })
                .UseWindowsService(opt => opt.ServiceName = "Ademund.OTC.DynamicIp");
    }
}
