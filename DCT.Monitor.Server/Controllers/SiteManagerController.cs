using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DCT.Monitor.Server.Helpers;
using DCT.Monitor.Server.Models;
using DCT.Monitor.Server.Security;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using DCT.Monitor.Modules.Implementation.PageModule;
using DCT.Monitor.Modules.Implementation.ProviderModule;
using DCT.Monitor.Modules.Implementation.SiteManager;
using DCT.Monitor.Modules.Implementation.UserModule;

namespace DCT.Monitor.Server.Controllers
{
	[Authorize]
	public class SiteManagerController : BaseController
	{
		IUserModule userModule = new UserModule();
		IProviderModule providerModule = new ProviderModule();
		ISiteManagerModule siteModule = new SiteManagerModule();
        IPageModule pageModule = new PageModule();

		[ActionName("Content")]
		public ActionResult ContentAction(string view)
		{
			return View(view);
		}

		[ActionName("create")]
		public ActionResult CreateSite(string domain)
		{
            if (Session.User != null)
            {
                var site = new Site() { Domain = domain };
                return Redirect(Url.CreateSite());
            }
            else
            {
                return Redirect(Url.Logon());
            }
		}

        [ActionName("Sites"), HttpGet]
        public ActionResult Sites()
        {
            SiteListModel listOfSites = new SiteListModel();
            listOfSites.Sites = siteModule.GetSitesByUser(Session.User);
            listOfSites.Session = Session;
            ViewData.Model = listOfSites;

            return View("SitesNew");
        }

		[ActionName("Sites"), HttpPost]
		public ActionResult Sites(SiteListModel model)
		{
			if (ModelState.IsValid)
			{
				var site = new Site() {Domain = model.SiteModel.Domain, ContainsSubdomains = model.SiteModel.IgnoreSubdomains };
				siteModule.CreateSite(Session.User, site);
					
				model.SiteModel.Domain = string.Empty;
                
                return Redirect(Url.Sites());
			}

			return Sites();
		}

		public ActionResult DeleteSite(Guid siteId)
		{
			if (Session.User != null)
			{
				siteModule.Delete(siteId);
				return Redirect(Url.Sites());
			}
			else
			{
				return Redirect(Url.Logon());
			}
		}

		[HttpPost, Authorize]
		public ActionResult EditSite(SiteListModel model)
		{
			if (Session.User != null)
			{
				siteModule.EditDomain( new Site()
										{
											Id = model.SiteModel.Id, 
											UserId = Session.User.Id,
											Domain = model.SiteModel.Domain, 
											ContainsSubdomains = model.SiteModel.IgnoreSubdomains
										});
				return Redirect(Url.Sites());
			}
			else
			{
				return Redirect(Url.Logon());
			}
		}

        public ActionResult EditPages(Guid siteId)
        {
            List<Page> pages = pageModule.GetPagesBySiteId(siteId);

            return View(pages);
        }

        public ActionResult DeletePage(Guid pageId)
        {   
            var page = pageModule.GetPageById(pageId);
            pageModule.DeletePage(page);

            return Redirect(Url.Pages(page.DomainId)); 
        }
                
        [HttpPost]
        public ActionResult EditPage(Guid id)
        {
            var pagePattern = Request["Page_" + id];

            var page = pageModule.GetPageById(id);
            page.PagePattern = pagePattern;
            pageModule.UpdatePage(page);

            return Redirect(Url.Pages(page.DomainId));
        }
        

        [HttpPost]
        public ActionResult AddPage()
        {
            var pagePattern = Request["PageModel.Page"];
            var siteId = Guid.Parse(Request["siteId"]);

            var page = new Page() { DomainId = siteId, PagePattern = pagePattern };
            pageModule.CreatePage(page);

            return Redirect(Url.Pages(siteId));
        }

        [ActionName("Pages"), HttpGet]
        public ActionResult Pages(Guid siteId)
        {            
            var listOfSites = new PageListModel();
            listOfSites.Pages = pageModule.GetPagesBySiteId(siteId);
            listOfSites.Session = Session;
            listOfSites.Site = siteModule.GetSite(siteId);
            ViewData.Model = listOfSites;
            ViewData.Add("SiteId",siteId);

            return View();           
        }
	}
}
