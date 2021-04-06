using System;

namespace Ademund.OTC.DMSUtils
{
    public class MessagesCompletedEventArgs : EventArgs
    {
        public bool Cancel { get; set; }

        public MessagesCompletedEventArgs(bool cancel = false)
        {
            Cancel = cancel;
        }
    }
}
