using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DCT.Monitor.Server.Helpers;
using DCT.Monitor.Server.Models;

namespace DCT.Monitor.Server.Security
{
	public class MonatorAuthorizeAttribute : AuthorizeAttribute
	{
		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.OnAuthorization(filterContext);

			if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
			{
				IFormsAuthenticationService FormsService = new FormsAuthenticationService();
				SessionHelper session = new SessionHelper(filterContext.HttpContext);

				if (session.SessionEndTime == null)
					session.SessionEndTime = DateTime.Now;
				else if (DateTime.Now - session.SessionEndTime > TimeSpan.FromMinutes(1))
				{
					FormsService.SignOut();
					filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "Logon" }));
				}
			}
		}
	}
}