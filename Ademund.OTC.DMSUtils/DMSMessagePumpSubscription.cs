﻿using Ademund.OTC.Client;
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
        private readonly System.Threading.CancellationToken _cancellationToken;
        private readonly IOTCDMSApi _api;
        private readonly IMessageProcessor _messageProcessor;
        private readonly Timer _timer;
        public string QueueId { get; }
        public string ConsumerGroupId { get; }
        public int BatchSize { get; }
        public int PollInterval { get; }

        public DMSMessagePumpSubscription(
            IOTCDMSApi api,
            IMessageProcessor messageProcessor,
            string queueId,
            string consumerGroupId,
            int pollInterval,
            int batchSize = 10,
            System.Threading.CancellationToken cancellationToken = default)
        {
            _api = api;
            _messageProcessor = messageProcessor;
            _cancellationToken = cancellationToken;

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
            while (true)
            {
                if (_cancellationToken.IsCancellationRequested)
                    return;

                try
                {
                    var response = await _api.ConsumeMessagesAsync(QueueId, ConsumerGroupId, BatchSize, cancellationToken: _cancellationToken).ConfigureAwait(false);
                    var responses = response.ToList();
                    if (responses.Count == 0)
                    {
                        _timer.Start();
                        return;
                    }

                    await AkkMessages(responses).ConfigureAwait(false);
                    var messages = new List<DMSMessage>(responses.Select(x => x.Message));
                    if (_messageProcessor is IMessageProcessorSync messageProcessorSync)
                    {
                        messageProcessorSync.Process(messages);
                    }
                    else if (_messageProcessor is IMessageProcessorAsync messageProcessorAsync)
                    {
                        await messageProcessorAsync.ProcessAsync(messages).ConfigureAwait(false);
                    }
                }
                catch (TaskCanceledException) { }
                catch (Exception ex)
                {
                    try
                    {
                        // TODO: this needs a logger to log the exception
                        var exMessage = new List<DMSMessage>() {
                            new DMSMessage() {
                                Body = new ExceptionMessage(null, ex),
                                Attributes = new Dictionary<string, string>() { { "Type", typeof(ExceptionMessage).Name } }
                            }
                        };

                        if (_messageProcessor is IMessageProcessorSync messageProcessorSync)
                        {
                            messageProcessorSync.Process(exMessage);
                        }
                        else if (_messageProcessor is IMessageProcessorAsync messageProcessorAsync)
                        {
                            await messageProcessorAsync.ProcessAsync(exMessage).ConfigureAwait(false);
                        }
                    }
                    catch
                    {
                        // TODO: this should probably trigger an on error event back in the client
                    }
                }
            }
        }

        private async Task AkkMessages(List<DMSConsumeMessageResponse> responses)
        {
            var messagesAck = new DMSMessagesAck() {
                Messages = responses.Select(x => new DMSMessageAck() {
                    Handler = x.Handler,
                    Status = DMSMessageAckStatus.Success
                }).ToArray()
            };
            var response = await _api.AckMessagesAsync(QueueId, ConsumerGroupId, messagesAck, cancellationToken: _cancellationToken).ConfigureAwait(false);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private record ExceptionMessage(object OriginalMessage, Exception Ex);
    }
}
