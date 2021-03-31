using System;

namespace Ademund.OTC.DMSUtils
{
    public interface IDMSMessagePumpSubscription
    {
        event EventHandler<SubscriptionOnMessagesEventArgs> OnMessagesReceived;
        event EventHandler<MessagesCompletedEventArgs> OnMessagesCompleted;
        event EventHandler<ProcessMessageEventArgs> OnProcessMessage;
        string QueueId { get; }
        int PollInterval { get; }
        void Start();
        void Stop();
    }
}
