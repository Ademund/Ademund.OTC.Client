using Ademund.OTC.Client.Model;
using System;
using System.Collections.Generic;

namespace Ademund.OTC.DMSUtils
{
    internal class DMSMessageHandler
    {
        private readonly Queue<DMSMessage> _messages = new Queue<DMSMessage>();
        private event EventHandler OnMessagesReceived;
        private bool _cancelled;

        public event EventHandler<MessagesCompletedEventArgs> OnMessagesCompleted;
        public event EventHandler<ProcessMessageEventArgs> OnProcessMessage;
        internal event EventHandler<ProcessMessageErrorEventArgs> OnProcessMessageError;

        public DMSMessageHandler()
        {
            OnMessagesReceived += DMSMessageHandler_OnMessagesReceived;
        }

        public void Start()
        {
            _cancelled = false;
        }

        public void Stop()
        {
            _cancelled = true;
        }

        public void HandleMessages(IEnumerable<DMSMessage> messages)
        {
            foreach (var message in messages)
            {
                if (_cancelled)
                    break;
                _messages.Enqueue(message);
            }
            OnMessagesReceived?.Invoke(this, default);
        }

        private void DMSMessageHandler_OnMessagesReceived(object sender, EventArgs e)
        {
            if (_cancelled)
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

                while (!cancel && !_cancelled && (_messages.Count > 0))
                {
                    cancel = ProcessMessage(_messages.Dequeue());
                }

                if (_cancelled)
                    return;

                if (cancel)
                    return;

                var args = new MessagesCompletedEventArgs();
                OnMessagesCompleted?.Invoke(this, args);
                cancel = args.Cancel;
            }
            finally
            {
                if (cancel || _cancelled)
                {
                    Stop();
                }
                else
                {
                    Start();
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
