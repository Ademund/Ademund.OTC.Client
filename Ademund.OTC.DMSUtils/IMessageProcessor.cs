using Ademund.OTC.Client.Model;

namespace Ademund.OTC.DMSUtils
{
    public interface IMessageProcessor
    {
        void Process(DMSMessage message);
    }
}
