
using System.Web.Mvc;
using System.Web;
using System;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Server.Helpers
{
	public static class HomeNavigationHelper
	{
		public static RouteHelper Home(this UrlHelper url) 
		{
			return new RouteHelper(url, "Home", null);
		}
        public static RouteHelper Downloads(this UrlHelper url)
        {
            return new RouteHelper(url, "Clients", null);
        }
		public static RouteHelper About(this UrlHelper url)
		{
			return new RouteHelper(url, "HomeContent", new { view = "About" });
		}
		public static RouteHelper ContactUs(this UrlHelper url)
		{
			return new RouteHelper(url, "Default", new { controller = "Home", action = "ContactUs" });
		}
		public static RouteHelper Faq(this UrlHelper url)
		{
			return new RouteHelper(url, "Default", new { controller = "Home", action = "Faq" });
		}
	}

	public static class SiteManagerNavigationHelper
	{
		public static RouteHelper CreateSite(this UrlHelper url)
		{
			return new RouteHelper(url, "SiteManagerContent", new { view = "CreateSite" });
		}

		public static RouteHelper CreateSiteComplite(this UrlHelper url)
		{
			return new RouteHelper(url, "SiteManager", new { action="create" });
		}
	
		public static RouteHelper Install(this UrlHelper url)
		{
			return new RouteHelper(url, "SiteManagerContent", new { view = "Install" });
		}

        public static RouteHelper Sites(this UrlHelper url)
        {
			return new RouteHelper(url, "SiteManager", new { action = "Sites" });
        }

		public static string StringScript(this UrlHelper url, string id)
		{
            var host = HttpContext.Current.Request.Url.Host;
			return "<script type=\"text/javascript\" src=\"http://" + host + "/Service/Script.js?SiteId=" + id + "\"></script>";
		}

		public static RouteHelper DeleteSite(this UrlHelper url, Site site)
		{
			return new RouteHelper(url, "SiteManager", new { action = "DeleteSite", siteId = site.Id });
		}

		public static RouteHelper EditSite(this UrlHelper url, Site site)
		{
			return new RouteHelper(url, "SiteManager", new { action = "EditSite", siteid = site.Id});
		}

        public static RouteHelper GetPages(this UrlHelper url, Guid siteId)
        {
            return new RouteHelper(url, "SiteManager", new { action = "Pages", SiteId = siteId });
        }

        public static RouteHelper DeletePage(this UrlHelper url, Page page)
        {
            return new RouteHelper(url, "SiteManager", new { action = "DeletePage", pageId = page.Id });
        }

        public static RouteHelper EditPage(this UrlHelper url, Page page)
        {
            return new RouteHelper(url, "SiteManager", new { action = "EditPage", Pageid = page.Id });
        }

        public static RouteHelper Pages(this UrlHelper url, Guid siteId)
        {
            return new RouteHelper(url, "SiteManager", new { action = "Pages", SiteId = siteId });
        }

	}

	public static class AccountNavigationHelper
	{
		public static RouteHelper MyHome(this UrlHelper url)
		{
			return new RouteHelper(url, "MyHome", null);
		}
		public static RouteHelper Logon(this UrlHelper url)
		{
			return new RouteHelper(url, "Default", new { controller = "Account", action = "LogOn" });
		}

        public static RouteHelper Logon(this UrlHelper url, string site)
        {
            return new RouteHelper(url, "OpenIdLogon", new { site = site });
        }

		public static RouteHelper Logoff(this UrlHelper url)
		{
			return new RouteHelper(url, "Default", new { controller = "Account", action = "LogOff" });
		}

		public static RouteHelper Register(this UrlHelper url)
		{
			return new RouteHelper(url, "Default", new { controller = "Account", action = "Register" });
		}

		public static RouteHelper ChangePassword(this UrlHelper url)
		{
			return new RouteHelper(url, "Default", new { controller = "Account", action = "ChangePassword" });
		}

		public static RouteHelper AccountBinding(this UrlHelper url)
		{
			return new RouteHelper(url, "Default", new { controller = "Account", action = "AccountBinding" });
		}

        public static RouteHelper RetrivePassword(this UrlHelper url)
        {
            return new RouteHelper(url, "Default", new { controller = "Account", action = "ForgotPassword" });
        }
	}

	public static class AdminNavigationHelper
	{
		public static RouteHelper ViewAdmin(this UrlHelper url)
		{
			return new RouteHelper(url, "Admin", new { controller = "Admin", action = "ViewAdmin"});
		}

		public static RouteHelper EditUser(this UrlHelper url, User user)
		{
			return new RouteHelper(url, "Admin", new { action = "EditUser", userid = user.Id});
		}
	}
}