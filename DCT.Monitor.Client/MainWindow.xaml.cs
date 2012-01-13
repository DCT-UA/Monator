using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DCT.Monitor.Client.Controllers;
using DCT.Monitor.Client.Models;

namespace DCT.Monitor.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                Icon = new BitmapImage(new Uri("pack://application:,,,/;component/Monator.ico"));
            }
            catch(Exception e)
            {
                e.ToString();
                // ignore for xp
            }

            _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += RefreshTimer_Tick;

            ViewsHost.SetStartupModel(new LogonModel());

            Dispatcher.BeginInvoke(new Action(() =>
            {
                _timer.Start();
            }), DispatcherPriority.Normal);
        }

        private long GetTotalMemory()
        {
            return GC.GetTotalMemory(false);
        }

        public void RefreshTimer_Tick(object sender, EventArgs e)
        {
            var top = ViewsHost.CurrentView as StatisticsModel;
            if(top != null) Commands.RefreshStatisticsCommand.Execute(top);
        }
    }
}
