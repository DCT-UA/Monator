using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using DCT.Monitor.Server.Helpers;
using System.Configuration;
using DCT.Monitor.Server.Models;

namespace DCT.Monitor.Server.Controllers
{
	public abstract class BaseController : Controller
	{
        public static Guid SelfSiteId { get; private set; }

		public new SessionHelper Session { get; private set; }

        static BaseController()
        {
            try
            {
                SelfSiteId = Guid.Parse(ConfigurationManager.AppSettings["SelfSiteId"]);
            }
            catch
            {
                throw new Exception(ConfigurationManager.AppSettings["SelfSiteId"]);
            }
        }

		public RedirectResult Redirect(RouteHelper helper, bool requireHttps = false)
		{
			return Redirect(helper.AsUrl().ToHtmlString());
		}

		protected override void Execute(System.Web.Routing.RequestContext requestContext)
		{
			Session = new SessionHelper(requestContext.HttpContext);

			if (!requestContext.HttpContext.Request.Url.AbsoluteUri.ToLower().Contains("service"))
			{

				if (requestContext.HttpContext.Request.IsAuthenticated)
				{
					IFormsAuthenticationService FormsService = new FormsAuthenticationService();

					if (DateTime.Now - Session.SessionEndTime > TimeSpan.FromMinutes(20))
					{
						FormsService.SignOut();
						requestContext.HttpContext.Response.Redirect(requestContext.HttpContext.Request.Url.AbsoluteUri, true);
					}
					else
					{
						Session.SessionEndTime = DateTime.Now;
					}
				}	
			}

			base.Execute(requestContext);
		}
	}
}
