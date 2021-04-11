using Ademund.OTC.Client;
using System.Collections.Concurrent;
using System.Threading;

namespace Ademund.OTC.DMSUtils
{
    public class DMSMessagePump : IDMSMessagePump
    {
        private readonly IOTCDMSApi _api;
        private readonly IMessageProcessor _messageProcessor;
        private readonly CancellationToken _cancellationToken;
        private readonly ConcurrentDictionary<string, IDMSMessagePumpSubscription> _subscriptions = new();
        private readonly ManualResetEvent _resetEvent = new(false);

        public DMSMessagePump(IOTCDMSApi api, IMessageProcessor messageProcessor, CancellationToken cancellationToken)
        {
            _api = api;
            _messageProcessor = messageProcessor;
            _cancellationToken = cancellationToken;
        }

        public void Pause()
        {
            // stop timers but do not signal the reset event
            foreach (var subscription in _subscriptions.Values)
                subscription.Stop();
        }

        public void Start()
        {
            foreach (var subscription in _subscriptions.Values)
                subscription.Start();
        }

        public void Wait()
        {
            _resetEvent.WaitOne();
        }

        public void Stop()
        {
            foreach (var subscription in _subscriptions.Values)
                subscription.Stop();
            if (!_resetEvent.SafeWaitHandle.IsClosed)
                _resetEvent.Set();
        }

        public IDMSMessagePumpSubscription Subscribe(string queueId, string consumerGroupId, int pollInterval)
        {
            string subscriptionKey = $"{queueId}/{consumerGroupId}";
            return _subscriptions.GetOrAdd(subscriptionKey, (_) =>
                new DMSMessagePumpSubscription(_api, _messageProcessor, queueId, consumerGroupId, pollInterval, cancellationToken: _cancellationToken));
        }

        public void UnSubscribe(string queueId, string consumerGroupId)
        {
            string subscriptionKey = $"{queueId}/{consumerGroupId}";
            _subscriptions.TryRemove(subscriptionKey, out _);
        }
    }
}
