using Ademund.OTC.Client;
using Ademund.OTC.Client.Model;
using Ademund.OTC.DynamicIp.Config;
using Ademund.OTC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Ademund.OTC.DynamicIp
{
    internal class IPChecker : IIPChecker
    {
        private readonly ISystrayMenu Systray;
        private readonly DynamicIpConfig Config;
        private readonly ILogger<IPChecker> Logger;
        private readonly string UserKey;
        private readonly CurrentIP PrevIP;

        public IPChecker(DynamicIpConfig config, ILogger<IPChecker> logger, ISystrayMenu systrayMenu, CurrentIP currentIP)
        {
            Config = config;
            Logger = logger;
            Systray = systrayMenu;
            PrevIP = currentIP;

            var machineName = Environment.MachineName;
            string macAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();
            string userName = Environment.UserName;
            UserKey = $"{machineName}:{macAddress}:{userName}";
            Logger.LogDebug($"IPChecker started - userKey: {UserKey}");
        }

        public async Task CheckIp(bool userMenuCheck = false)
        {
            string userIp;
            using var httpClient = new HttpClient();
            try
            {
                userIp = await httpClient.GetStringAsync("https://api.ipify.org").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Systray.ShowBalloonError("Fetch Error", $"There was an error fetching your ip address from ipify: {ex.Message}");
                Logger.LogError(ex, $"Error fetching ip address from ipify: {ex.Message}");
                return;
            }

            if (userMenuCheck || string.IsNullOrWhiteSpace(PrevIP.IP))
            {
                Systray.ShowBalloonTip("Current IP", $"IP: {userIp}");
            }

            if (!string.IsNullOrWhiteSpace(PrevIP.IP))
            {
                if (userIp?.Equals(PrevIP.IP) == true)
                {
                    Logger.LogDebug($"Ip has not changed since last check: {userIp}");
                    return;
                }

                if (!userMenuCheck && Config.ShowChangeNotifications)
                {
                    Systray.ShowBalloonTip("IP Change", $"IP changed from: {PrevIP.IP} to: {userIp}");
                }

                Logger.LogDebug($"Ip has changed since last check: prev={PrevIP.IP} => current={userIp}");
            }
            PrevIP.Update(userIp);

            foreach (var environment in Config.Environments)
            {
                try
                {
                    await CheckEnvironmentSecurityGroup(userIp, environment).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Systray.ShowBalloonError("Environment Error", $"There was an error checking the security group for: {environment.Name}");
                    Logger.LogError(ex, $"Error checking security group for: {environment.Name}, {ex.Message}");
                }
            }
        }

        private async Task CheckEnvironmentSecurityGroup(string userIp, EnvironmentConfig environment)
        {
            var signer = new Signer(environment.AccessKey, environment.SecretKey);
            using var client = new OTCApiClient(signer, Config.UseProxy ? Config.ProxyAddress : null);
            var api = client.InitOTCApi<IOTCVPCApi>($"https://vpc.{environment.Region}.otc.t-systems.com");
            api.ProjectId = environment.ProjectId;

            var response = await api.GetSecurityGroup(environment.ProjectId, environment.SecurityGroupId).ConfigureAwait(false);
            var ipRule = response.SecurityGroup.Rules
                .FirstOrDefault(r => r.RemoteIpPrefix?.StartsWith(userIp) == true);
            if (ipRule != null)
            {
                Logger.LogDebug($"Rule for ip: {userIp} already exists for environment: {environment.Name}");
                return;
            }

            var userRule = response.SecurityGroup.Rules
                .FirstOrDefault(r => !string.IsNullOrWhiteSpace(r.Description) && r.Description.Equals(UserKey));
            if (userRule == null)
            {
                Logger.LogDebug($"No rule exists for key: {UserKey}");
                var request = new SecurityGroupRuleRequest()
                {
                    SecurityGroupRule = new SecurityGroupRule()
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Description = UserKey,
                        Direction = "ingress",
                        EtherType = "IPv4",
                        Protocol = "tcp",
                        RemoteIpPrefix = $"{userIp}/32",
                        SecurityGroupId = response.SecurityGroup.Id,
                        TenantId = environment.ProjectId
                    }
                };
                var createResponse = await api.CreateSecurityGroupRule(environment.ProjectId, request).ConfigureAwait(false);
                Logger.LogDebug($"New rule added to environemnt: {environment.Name}, for key: {UserKey}, with ip: {userIp}");
            }
            else
            {
                Logger.LogDebug($"Ip has changed for key: {UserKey}");
                var request = new SecurityGroupRuleRequest()
                {
                    SecurityGroupRule = userRule with
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        RemoteIpPrefix = $"{userIp}/32"
                    }
                };
                await api.DeleteSecurityGroupRule(environment.ProjectId, userRule.Id).ConfigureAwait(false);
                await api.CreateSecurityGroupRule(environment.ProjectId, request).ConfigureAwait(false);
                Logger.LogDebug($"Rule updated for key: {UserKey}, with ip: {userIp}");
            }
        }
    }
}
