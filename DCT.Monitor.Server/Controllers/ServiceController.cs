using System.Configuration;
using System.Web.Mvc;
using System;
using System.Web;
using System.Linq;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.IO;
using DCT.Unity;
using System.Collections.Generic;
using DCT.Monitor.Server.Models;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace DCT.Monitor.Server.Controllers
{
	public class ServiceController : BaseController
	{
        private static IRequestModule requestModule = ServiceLocator.Current.Resolve<IRequestModule>();
        private static IDomainStatisticsDataSourceModule domainStatSource = ServiceLocator.Current.Resolve<IDomainStatisticsDataSourceModule>();
        private static ISiteManagerModule siteManagerModule = ServiceLocator.Current.Resolve<ISiteManagerModule>();
        private static IUserModule userModule = ServiceLocator.Current.Resolve<IUserModule>();
		private static ICacheManagerModule cacheModule = ServiceLocator.Current.Resolve<ICacheManagerModule>();
        private static IConfigurationModule config = ServiceLocator.Current.Resolve<IConfigurationModule>();

        public IFormsAuthenticationService FormsService { get; set; }
        public IAccountService AccountService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (AccountService == null) { AccountService = new AccountService(); }

            base.Initialize(requestContext);
        }

		public ActionResult Index(Guid? siteId)
		{
            if (siteId == null) throw new HttpException(404, "Site id not specified");
            ViewData.Model = new JsonpRequestModel { SiteId = siteId.Value };
			return View("Script");
		}

        public ContentResult Logon(string username, string password)
        {
            var currentUser = new User
            {
                UserName = username,
                Password = password
            };

            if (userModule.Authenticate(currentUser))
            {
                FormsService.SignIn(username, false);
                return Content("ok");
            }

            return Content("error");
        }

        public ActionResult Count()
        {
            if (!Request.IsAuthenticated) return null;

            IEnumerable<DomainStatistics> stats = null;

            if (Session.User.IsAdministrator)
                stats = domainStatSource.GetDomainStatistics();
            else
                stats = domainStatSource.GetDomainStatisticsByUser(Session.User);

            return Json(stats.Select(i => new { i.Id, i.Pattern, i.Count, i.ParentId }));
        }

        public ActionResult Requests(Guid id)
        {
            if (!Request.IsAuthenticated) 
                return null;

            var requests = domainStatSource.GetDomainRequests(new DomainSelector(id));

            if (requests == null) 
                requests = new List<PageRequest>();

            return Json(requests.Select(i => new { i.Url, Duration = i.DurationString, i.Browser, i.SessionIdentifier, i.Refferer, i.IpAddress, i.UserId, i.Domain }));
        }

        private PageRequest ProcessEvent(Guid siteId, Uri urlReferrer, Guid? requestId = null, TimeSpan? duration = null, string refferer = "")
        {
            Guid sessionId = Guid.Empty;
            var sessionCookie = Request.Cookies["SkynerSessionId"];
            if (sessionCookie != null) Guid.TryParse(sessionCookie.Value, out sessionId);

            if (sessionId == null || sessionId.Equals(Guid.Empty)) sessionId = Guid.NewGuid();

            var httpCookie = new HttpCookie("SkynerSessionId");
            httpCookie.Domain = Request.Url.Host;
            httpCookie.Value = sessionId.ToString();
            httpCookie.HttpOnly = true;
            Response.SetCookie(httpCookie);

            var request = new PageRequest
            {
                Id = requestId.GetValueOrDefault(Guid.NewGuid()),
                SessionIdentifier = sessionId,
                Url = urlReferrer.AbsolutePath,
                Domain = urlReferrer.Host,
                IpAddress = Request.UserHostAddress,
                Browser = (byte)Request.Browser.Browser.GetBrowserFromString(),
                Duration = duration.GetValueOrDefault(TimeSpan.Zero),
                Refferer = refferer,
                SiteId = siteId
            };

            try
            {
                SendEvent(request);
            }
            catch
            {
                throw new HttpException(404, "not found");
            }

            return request;
        }

        private void SendEvent(PageRequest evt)
        {
			evt.Domain = (evt.Domain ?? "").Replace("www.", "");
            requestModule.SendRequest(evt);
        }

        public ActionResult Test()
        {
            var cache = MvcApplication.Cache;
            return View(cache.Get(Request.Url.Host));
        }

        public ActionResult Log(int? index)
        {
            var idx = index.GetValueOrDefault(0);
            var files = Directory.GetFiles(Server.MapPath("/Logs"));
            if(files.Length == 0) return null;
            if(files.Length <= idx || idx < 0) return Content("filecount = " + files.Length, "text/plain");

            return File(files[idx], "text/plain", "log.txt");
        }

        private new ActionResult Json(object model)
        {
            var jsonSerializer = new JavaScriptSerializer();

            return Content(jsonSerializer.Serialize(model));
        }

		public ActionResult CleanCache()
		{
			if (Session.User.IsAdministrator)
			{
				cacheModule.Reset();
				return Content("ok!");
			}
			else
			{
				return Content("fail!");
			}
			
		}

        public string Delay(string delay)
        {
            if (!Session.User.IsAdministrator) return "error";

            if (delay != null) config.CoreDelay = int.Parse(delay);

            return config.CoreDelay.ToString();
        }

        public string Jsonp(JsonpRequestModel model)
        {
            var responseModel = new JsonpResponseModel();

            var pageRequest = ProcessEvent(model.SiteId, new Uri(model.Url), model.RequestId, TimeSpan.FromSeconds(model.Duration), model.Referer);

            if (model.JsonAction == "init")
            {
                responseModel.RequestId = pageRequest.Id;
                responseModel.SessionId = pageRequest.SessionIdentifier;
            }

            responseModel.Delay = config.CoreDelay;
            responseModel.Callback = model.Callback;

            Response.ContentType = "text/javascript";

            return responseModel.ToString();
        }
	}
}
