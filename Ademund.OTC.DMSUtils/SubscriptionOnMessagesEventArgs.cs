using System;

namespace Ademund.OTC.DMSUtils
{
    public class SubscriptionOnMessagesEventArgs : EventArgs
    {
        public int MessageCount { get; }
        public bool Ready { get; set; }
        public bool Cancel { get; set; }

        public SubscriptionOnMessagesEventArgs(int messageCount)
        {
            MessageCount = messageCount;
        }
    }
}
