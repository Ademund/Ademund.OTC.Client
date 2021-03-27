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
    internal class CurrentIP
    {
        public string IP { get; private set; } = string.Empty;
        public event EventHandler OnChanged;
        public void Update(string ip)
        {
            if (IP != ip)
            {
                IP = ip;
                OnChanged?.Invoke(this, default);
            }
        }
    }
}
