using Ademund.OTC.Client.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Ademund.OTC.DMSUtils
{
    internal class DMSMessageHandler
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly Queue<DMSMessage> _messages = new Queue<DMSMessage>();
        private event EventHandler OnMessagesReceived;

        public event EventHandler<MessagesCompletedEventArgs> OnMessagesCompleted;
        public event EventHandler<ProcessMessageEventArgs> OnProcessMessage;
        internal event EventHandler<ProcessMessageErrorEventArgs> OnProcessMessageError;

        public DMSMessageHandler(CancellationTokenSource tokenSource)
        {
            _tokenSource = tokenSource;
            OnMessagesReceived += DMSMessageHandler_OnMessagesReceived;
        }

        public void Start()
        {
        }

        public void Stop()
        {
            _tokenSource.Cancel();
        }

        public void HandleMessages(IEnumerable<DMSMessage> messages)
        {
            foreach (var message in messages)
            {
                if (_tokenSource.IsCancellationRequested)
                    break;
                _messages.Enqueue(message);
            }
            OnMessagesReceived?.Invoke(this, default);
        }

        private void DMSMessageHandler_OnMessagesReceived(object sender, EventArgs e)
        {
            if (_tokenSource.IsCancellationRequested)
                return;
            ProcessMessages();
        }

        private void ProcessMessages()
        {
            bool cancel = false;
            try
            {
                if (_messages.Count == 0)
                    return;

                while (!cancel && !_tokenSource.IsCancellationRequested && (_messages.Count > 0))
                {
                    cancel = ProcessMessage(_messages.Dequeue());
                }

                if (_tokenSource.IsCancellationRequested)
                    return;

                if (cancel)
                    return;

                var args = new MessagesCompletedEventArgs();
                OnMessagesCompleted?.Invoke(this, args);
                cancel = args.Cancel;
            }
            finally
            {
                if (!_tokenSource.IsCancellationRequested)
                {
                    if (cancel)
                    {
                        Stop();
                    }
                    else
                    {
                        Start();
                    }
                }
            }
        }

        private bool ProcessMessage(DMSMessage message)
        {
            try
            {
                var args = new ProcessMessageEventArgs(message);
                OnProcessMessage?.Invoke(this, args);
                return args.Cancel;
            }
            catch (Exception ex)
            {
                OnProcessMessageError?.Invoke(this, new ProcessMessageErrorEventArgs(message, ex));
                return false;
            }
        }
    }
}
