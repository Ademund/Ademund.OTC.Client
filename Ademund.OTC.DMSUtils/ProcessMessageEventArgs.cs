using Ademund.OTC.Client.Model;
using System;

namespace Ademund.OTC.DMSUtils
{
    public class ProcessMessageEventArgs : EventArgs
    {
        public DMSMessage Message { get; }
        public bool Cancel { get; set; }

        public ProcessMessageEventArgs(DMSMessage message, bool cancel = false)
        {
            Message = message;
            Cancel = cancel;
        }
    }
}
