using System;
using System.Runtime.InteropServices;

namespace Ademund.OTC.DynamicIp
{
    internal static class WindowHandler
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void Show(string title)
        {
            IntPtr hWnd = FindWindow(null, title);
            if (hWnd != IntPtr.Zero)
            {
                //Show window again
                ShowWindow(hWnd, 1);
            }
        }

        public static void Hide(string title)
        {
            IntPtr hWnd = FindWindow(null, title);
            if (hWnd != IntPtr.Zero)
            {
                //Hide the window
                ShowWindow(hWnd, 0);
            }
        }
    }
}
