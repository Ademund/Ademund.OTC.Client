using Ademund.OTC.DynamicIp.Config;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Ademund.OTC.DynamicIp
{
    internal class SystrayMenu : ISystrayMenu
    {
        private const string AppName = "Ademund.OTC.DynamicIp";
        private readonly DynamicIpConfig Config;
        private readonly CurrentIP CurrentIP;
        private TaskbarIcon tbi;

        public event EventHandler OnExit;
        public event EventHandler OnCheckNow;

        public SystrayMenu(DynamicIpConfig config, CurrentIP currentIP)
        {
            Config = config;
            CurrentIP = currentIP;
            CurrentIP.OnChanged += CurrentIP_OnChanged;
            CheckStartup();

            // create a thread  
            var newWindowThread = new Thread(new ThreadStart(() =>
            {
                // create and show the window
                var contextMenu = new ContextMenu();
                var menuCheckNow = new MenuItem()
                {
                    Header = "Check Now",
                    Icon = new Image { Source = Resources.Network.ToBitmap().ToBitmapImage() }
                };
                var menuQuit = new MenuItem()
                {
                    Header = "Quit",
                    Icon = new Image { Source = Resources.Warning.ToBitmap().ToBitmapImage() }
                };

                menuQuit.Click += MenuQuit_Click;
                menuCheckNow.Click += MenuCheckNow_Click;

                contextMenu.Items.Add(menuCheckNow);
                contextMenu.Items.Add(new Separator());
                contextMenu.Items.Add(menuQuit);

                tbi = new TaskbarIcon
                {
                    Icon = Resources.Network,
                    ToolTipText = "IPChecker",
                    ContextMenu = contextMenu,
                    MenuActivation = PopupActivationMode.LeftOrRightClick
                };

                // start the Dispatcher processing  
                System.Windows.Threading.Dispatcher.Run();
            }));

            // set the apartment state  
            newWindowThread.SetApartmentState(ApartmentState.STA);

            // make the thread a background thread  
            newWindowThread.IsBackground = true;

            // start the thread  
            newWindowThread.Start();
        }

        private void CurrentIP_OnChanged(object sender, EventArgs e)
        {
            tbi.Dispatcher.Invoke(() => tbi.ToolTipText = $"IP: {CurrentIP.IP}");
        }

        private void MenuCheckNow_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnCheckNow?.Invoke(this, default);
        }

        private void MenuQuit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnExit?.Invoke(this, default);
        }

        private void CheckStartup()
        {
            if (Config.StartMinimized)
                HideWindow();

            string currentExePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string keyValue = rk.GetValue(AppName, string.Empty).ToString();
            if (Config.AutoStart && (string.IsNullOrWhiteSpace(keyValue) || keyValue != currentExePath))
            {
                rk.SetValue(AppName, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            }
            else if (!string.IsNullOrWhiteSpace(keyValue) && !Config.AutoStart)
            {
                rk.DeleteValue(AppName, false);
            }
        }

        private void HideWindow()
        {
            WindowHandler.Hide(AppName);
        }

        private void ShowWindow()
        {
            WindowHandler.Show(AppName);
        }

        public void ShowBalloonTip(string title, string text)
        {
            tbi.Dispatcher.Invoke(() =>
            {
                tbi.ShowBalloonTip(title, text, BalloonIcon.Info);
                Thread.Sleep(3000);
                tbi.HideBalloonTip();
            });
        }

        public void ShowBalloonError(string title, string text)
        {
            tbi.Dispatcher.Invoke(() =>
            {
                tbi.ShowBalloonTip(title, text, BalloonIcon.Error);
                Thread.Sleep(3000);
                tbi.HideBalloonTip();
            });
        }

        public void Dispose()
        {
            tbi?.Dispose();
        }
    }

    public static class BitmapExtensions
    {
        public static BitmapImage ToBitmapImage(this System.Drawing.Bitmap bitmap)
        {
            using var memory = new MemoryStream();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}
