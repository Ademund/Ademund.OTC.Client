using System.Collections.Generic;

namespace Ademund.OTC.DynamicIp.Config
{
    public record DynamicIpConfig
    {
        public bool AutoStart { get; init; }
        public bool StartMinimized { get; init; }
        public int IntervalInMinutes { get; init; }
        public bool ShowChangeNotifications { get; init; }
        public string ProxyAddress { get; init; }
        public bool UseProxy { get; init; }
        public IEnumerable<EnvironmentConfig> Environments { get; init; }
    }
}
