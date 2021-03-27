using Ademund.OTC.DynamicIp.Config;
using FluentScheduler;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ademund.OTC.DynamicIp
{
    internal class IPCheckerService : IHostedService
    {
        private readonly ISystrayMenu Systray;
        private readonly IHostApplicationLifetime AppLifetime;
        private readonly ILogger<IPCheckerService> Logger;
        private readonly IIPChecker IPChecker;
        private readonly DynamicIpConfig Config;

        public IPCheckerService(IHostApplicationLifetime appLifetime, ILogger<IPCheckerService> logger, IIPChecker ipChecker, DynamicIpConfig config, ISystrayMenu systrayMenu)
        {
            Logger = logger;
            AppLifetime = appLifetime;
            IPChecker = ipChecker;
            Config = config;
            Systray = systrayMenu;
            Systray.OnCheckNow += Systray_OnCheckNow;
            Systray.OnExit += Systray_OnExit;
        }

        private void Systray_OnExit(object sender, System.EventArgs e)
        {
            JobManager.Stop();
            Environment.Exit(-1);
        }

        private void Systray_OnCheckNow(object sender, System.EventArgs e)
        {
            IPChecker.CheckIp(true);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            AppLifetime.ApplicationStarted.Register(OnStarted);
            AppLifetime.ApplicationStopping.Register(OnStopping);
            AppLifetime.ApplicationStopped.Register(OnStopped);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void OnStarted()
        {
            Logger.LogDebug($"IPCheckerService Started, configured to check every {Config.IntervalInMinutes} minutes");
            JobManager.Initialize();
            JobManager.AddJob(
                async () => await IPChecker.CheckIp().ConfigureAwait(false),
                s => s.ToRunNow().AndEvery(Config.IntervalInMinutes).Minutes()
            );
        }

        public void OnStopping()
        {
            Systray?.Dispose();
        }

        public void OnStopped()
        {
            Logger.LogDebug("IPCheckerService Stopped");
        }
    }
}