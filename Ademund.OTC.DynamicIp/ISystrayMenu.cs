using System;

namespace Ademund.OTC.DynamicIp
{
    internal interface ISystrayMenu : IDisposable
    {
        void ShowBalloonTip(string title, string text);
        void ShowBalloonError(string title, string text);
        event EventHandler OnExit;
        event EventHandler OnCheckNow;
    }
}