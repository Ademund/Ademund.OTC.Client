using Ademund.OTC.Client;
using Ademund.OTC.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Ademund.OTC.DMSUtils
{
    internal class DMSMessagePumpSubscription : IDMSMessagePumpSubscription
    {
        private readonly DMSMessageHandler _messageHandler;
        private readonly IOTCDMSApi _api;
        private readonly Timer _timer;
        private bool _cancelled;
        public string QueueId { get; }
        public string ConsumerGroupId { get; }
        public int BatchSize { get; }
        public int PollInterval { get; }

        public event EventHandler<SubscriptionOnMessagesEventArgs> OnMessagesReceived;
        public event EventHandler<MessagesCompletedEventArgs> OnMessagesCompleted;
        public event EventHandler<ProcessMessageEventArgs> OnProcessMessage;

        public DMSMessagePumpSubscription(IOTCDMSApi api, string queueId, string consumerGroupId, int pollInterval, int batchSize = 10)
        {
            _api = api;
            _messageHandler = new DMSMessageHandler();
            _messageHandler.OnMessagesCompleted += MessageHandler_OnMessagesCompleted;
            _messageHandler.OnProcessMessage += MessageHandler_OnProcessMessage;
            _messageHandler.OnProcessMessageError += MessageHandler_OnProcessMessageError;
            _timer = new Timer(pollInterval) {
                AutoReset = false
            };
            _timer.Elapsed += Timer_Elapsed;

            QueueId = queueId;
            ConsumerGroupId = consumerGroupId;
            BatchSize = batchSize;
            PollInterval = pollInterval;
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await ConsumeMessages().ConfigureAwait(false);
        }

        private async Task ConsumeMessages()
        {
            if (_cancelled)
                return;

            if (OnMessagesReceived?.GetInvocationList()?.Length == 0)
            {
                _timer.Start();
                return;
            }

            var response = await _api.ConsumeMessages(QueueId, ConsumerGroupId, BatchSize).ConfigureAwait(false);
            var responses = response.ToList();
            var eventArgs = new SubscriptionOnMessagesEventArgs(responses.Count);
            if (responses.Count == 0)
            {
                OnMessagesReceived?.Invoke(this, eventArgs);
                if (eventArgs.Cancel)
                {
                    Stop();
                }
                else
                {
                    _timer.Start();
                }
                return;
            }

            var messages = new List<DMSMessage>(responses.Select(x => x.Message));
            OnMessagesReceived?.Invoke(this, eventArgs);
            if (!eventArgs.Ready)
            {
                _timer.Start();
                return;
            }

            await AkkMessages(responses).ConfigureAwait(false);
            _messageHandler.HandleMessages(messages);
        }

        private async Task AkkMessages(List<DMSConsumeMessageResponse> responses)
        {
            var messagesAck = new DMSMessagesAck() {
                Messages = responses.Select(x => new DMSMessageAck() {
                    Handler = x.Handler,
                    Status = DMSMessageAckStatus.success
                }).ToArray()
            };
            var response = await _api.AckMessages(QueueId, ConsumerGroupId, messagesAck).ConfigureAwait(false);
        }

        private void MessageHandler_OnProcessMessage(object sender, ProcessMessageEventArgs e)
        {
            OnProcessMessage?.Invoke(sender, e);
            if (e.Cancel)
            {
                Stop();
            }
        }

        private void MessageHandler_OnProcessMessageError(object sender, ProcessMessageErrorEventArgs e)
        {
            // TODO: put the message on an error queue
        }

        private async void MessageHandler_OnMessagesCompleted(object sender, MessagesCompletedEventArgs e)
        {
            OnMessagesCompleted?.Invoke(sender, e);
            if (e.Cancel)
            {
                Stop();
            }
            else
            {
                await ConsumeMessages().ConfigureAwait(false);
            }
        }

        public void Start()
        {
            _messageHandler.Start();
            _timer.Start();
        }

        public void Stop()
        {
            _cancelled = true;
            _messageHandler.Stop();
            _timer.Stop();
        }
    }
}
