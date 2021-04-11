using System;

namespace Ademund.OTC.DMSUtils
{
    public interface IDMSMessagePumpSubscription
    {
        string QueueId { get; }
        int PollInterval { get; }
        void Start();
        void Stop();
    }
}
