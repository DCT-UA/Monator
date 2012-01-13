using System;
using System.Windows;
using DCT.Monitor.Cache;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using DCT.Monitor.ServiceDataSource;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using System.Runtime.ExceptionServices;
using DCT.Unity;

namespace DCT.Monitor.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public User CurrentUser { get; private set; }

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += FirstChanceException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var resourceDictionaryTheme = new ResourceDictionary { Source = new Uri("Styles/" + ThemeName + "/Theme.xaml", UriKind.Relative) };
            base.Resources.MergedDictionaries.Clear();
            base.Resources.MergedDictionaries.Add(resourceDictionaryTheme);

            base.OnStartup(e);
        }

        public void LogonUser(User user)
        {
			CurrentUser = user;
        }

		public void Logout(User user)
		{
			CurrentUser = null;
		}

        private static void FirstChanceException(object o, FirstChanceExceptionEventArgs e)
        {
            if (e.Exception is UnityConfigurationException)
            {
                var container = ((UnityConfigurationException)e.Exception).Container;

                container.RegisterInstance<ICache>(new DictionaryCache());
                container.RegisterType(typeof(IDomainStatisticsDataSourceModule), typeof(JsonDataSource));
                container.RegisterType(typeof(IUserModule), typeof(JsonUserModule));
            }
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            OnError(args.ExceptionObject);
        }

        private static void OnError(object e)
        {
            Logger.Write(e);
            Logger.FlushContextItems();
        }

        public static App CurrentApp { get { return (App)Current; } }
        public string ThemeName { get { return "ExpressionDark"; } }
    }
}
