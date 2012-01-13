using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DCT.Monitor.Cache;
using DCT.Monitor.Modules;
using DCT.Monitor.Modules.Implementation.DataSource;
using DCT.Monitor.Modules.Implementation.DomainStatisticsModule;
using DCT.Monitor.Modules.Implementation.GeolocationModule;
using DCT.Monitor.Modules.Implementation.ProviderModule;
using DCT.Monitor.Modules.Implementation.RequestModule;
using DCT.Monitor.Modules.Implementation.SiteManager;
using DCT.Monitor.Modules.Implementation.UserModule;
using DCT.Monitor.StreamInsight;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Web.Configuration;
using System.Threading;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Monitor.DAL.Implementation.Requests;
using DCT.Unity;
using System.Runtime.ExceptionServices;
using My.SqlEngine;
using Monitor.DAL;
using Monitor.DAL.Implementation.Users;
using Monitor.DAL.Implementation.Sites;
using Monitor.DAL.Implementation.Geolocations;
using Monitor.DAL.Implementation.Providers;
using DCT.Monitor.Server.Helpers;
using DCT.Monitor.Server.Models;


namespace DCT.Monitor.Server
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
        public static ICache Cache { get; set; }

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("robots.txt");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("dataservise.svc/{*pathInfo}");
            routes.IgnoreRoute("StreamInsight.svc/{*pathInfo}");

			//routes.MapRoute(
			//    "Register",
			//    "Register/{view}",
			//    new { controller = "Account", action = "Register", view = "Index" }
			//);

			routes.MapRoute(
				"Script",
				"service/script.js",
				new { controller = "Service", action = "Index" }
			);


			routes.MapRoute(
				"Home",
				"",
				new { controller = "Home", action = "Content", view = "Index" }
			);

			routes.MapRoute(
				"Admin",
				"admin/{action}",
				new { controller = "Admin", action = "ViewAdmin" }
			);

			routes.MapRoute(
				"ContactUs",
				"home/contactus",
				new { controller = "Home", action = "ContactUs" }
			);

			routes.MapRoute(
				"HomeContent",
				"home/{view}",
				new { controller = "Home", action = "Content", view = "Index" }
			);

			routes.MapRoute(
				"SiteManagerContent",
				"my/help/{view}",
				new { controller = "SiteManager", action = "Content", view = "Install" }
			);

			routes.MapRoute(
				"SiteManager",
				"my/{action}",
				new { controller = "SiteManager" }
			);

            routes.MapRoute(
                "Users",
                "user/{action}",
                new { controller = "Service" }
                );

            routes.MapRoute(
                "Clients",
                "downloads",
                new { controller = "Home", action = "Content", view = "Clients" }
                );

			routes.MapRoute(
				"MyHome",
				"myhome",
				new { controller = "Account", action = "MyHome", view = "MyHome" }
				);

            routes.MapRoute(
                    "OpenIdLogon", // Route name
                    "Account/Logon/{site}", // URL with parameters
                    new { controller = "Account", action = "LogonUsingOpenId", } // Parameter defaults
                );
			routes.MapRoute(
				   "EditUser", // Route name
				   "my/{action}", // URL with parameters
				   new { controller = "SiteManager", action = "EditSite" } // Parameter defaults
			   );
            routes.MapRoute(
                   "Pages", // Route name
                   "my/{action}", // URL with parameters
                   new { controller = "SiteManager", action = "Pages", siteId = UrlParameter.Optional } // Parameter defaults
               );

			//routes.MapRoute(
			//       "Facebook", // Route name
			//       "Account/{site}", // URL with parameters
			//       new { controller = "Account", action = "Facebook", id = UrlParameter.Optional } // Parameter defaults
			//   );

			routes.MapRoute(
					"Default", // Route name
					"{controller}/{action}/{id}", // URL with parameters
					new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			    );

		}

		protected void Application_Start()
		{
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += UnityConfigHandler;

			AreaRegistration.RegisterAllAreas();
			RegisterRoutes(RouteTable.Routes);

            ServiceLocator.LoggerService.Info("Start App");
            var cache = ServiceLocator.Current.Resolve<ICache>();
            Cache = cache;			
        }

        private void UnityConfigHandler(object sender, FirstChanceExceptionEventArgs e)
        {
            var exception = e.Exception as UnityConfigurationException;
            if (exception == null) return;
            ServiceLocator.LoggerService.Error(e.Exception);
        }

        private void UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            OnError((Exception)args.ExceptionObject);
        }

        protected void Application_Error()
        {
            var e = Server.GetLastError();
            OnError(e);
        }

        private void OnError(Exception e)
        {
            ServiceLocator.LoggerService.Error(e);
        } 
    }
}