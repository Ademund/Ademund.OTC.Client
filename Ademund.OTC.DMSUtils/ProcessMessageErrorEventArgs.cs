using Ademund.OTC.Client.Model;
using System;

namespace Ademund.OTC.DMSUtils
{
    public class ProcessMessageErrorEventArgs : EventArgs
    {
        public Exception Ex { get; }
        public DMSMessage Message { get; }

        public ProcessMessageErrorEventArgs(DMSMessage message, Exception ex)
        {
            Message = message;
            Ex = ex;
        }
    }
}
