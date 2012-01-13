using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DCT.Monitor.Server.Models;
using DCT.Monitor.Server.Helpers;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using DCT.Monitor.Modules.Implementation.SiteManager;
using DCT.Monitor.Modules.Implementation.UserModule;

namespace DCT.Monitor.Server.Controllers
{
	[HandleError]
    public class AdminController : BaseController
    {
		IUserModule userModule = new UserModule();
		ISiteManagerModule siteModule = new SiteManagerModule();
        

        public ActionResult Index()
        {
            return View();
        }

		public ActionResult ViewAdmin()
		{
			AdminUserListModel listOfUsers = new AdminUserListModel();
			listOfUsers.Users = userModule.GetAll();
			listOfUsers.UserSitesString = new List<string>();
			foreach( User user in  listOfUsers.Users)
			{
				List<Site> sites = siteModule.GetSitesByUser(user);
				string tempString = string.Empty;
				foreach(Site site in sites)
				{
					tempString = tempString + site.Domain + ", ";
				}
				listOfUsers.UserSitesString.Add(tempString);
			}
			listOfUsers.Session = Session;
			return View(listOfUsers);
		}       
        
        public ActionResult EditUser(Guid userId)
        {            
            User user = new User() { Id = userId };
            List<Site> sites = siteModule.GetSitesByUser(user);
            
            return View(sites);
        }

        [HttpPost]
        public ActionResult EditSite(Guid id)
        {
            var siteText = Request["Domain_" + id];
            
            var site = siteModule.GetSite(id);
            site.Domain = siteText;
            siteModule.EditDomain(site);

            return Redirect(Url.ViewAdmin());
        }
    }
}
