using Ademund.OTC.Client.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ademund.OTC.DMSUtils
{
    public interface IMessageProcessor
    {
    }

    public interface IMessageProcessorSync : IMessageProcessor
    {
        void Process(DMSMessage message);
        void Process(IEnumerable<DMSMessage> messages);
    }

    public interface IMessageProcessorAsync : IMessageProcessor
    {
        Task ProcessAsync(DMSMessage message);
        Task ProcessAsync(IEnumerable<DMSMessage> messages);
    }
}
