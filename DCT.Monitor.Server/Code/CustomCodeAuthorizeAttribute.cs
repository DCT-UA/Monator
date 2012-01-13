using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DCT.Monitor.Server.Models;
using System.Web.Configuration;

namespace DCT.Monitor.Server.Code
{
	public class CustomCodeAuthorizeAttribute : AuthorizeAttribute
	{
		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			//var connectionString = WebConfigurationManager.ConnectionStrings["JournalsConnectionString"].ConnectionString;
			//using (var repository = new JournalsDataContext(connectionString))
			//{
			//    var code = repository.Cards.FirstOrDefault(x => x.Id == Convert.ToInt32(httpContext.Session["CardId"]));
			//    if (code != null)
			//    {
			//        return true;
			//    }
			//}

			return false;
		}
	}
}