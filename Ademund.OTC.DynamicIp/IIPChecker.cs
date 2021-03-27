using System.Threading.Tasks;

namespace Ademund.OTC.DynamicIp
{
    internal interface IIPChecker
    {
        Task CheckIp(bool userMenuCheck = false);
    }
}
