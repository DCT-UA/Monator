using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DCT.Monitor.Server.Helpers;
using DCT.Monitor.Server.Models;
using DCT.Monitor.Server.Extensions;
using System.Text.RegularExpressions;

namespace DCT.Monitor.Server.Controllers
{
	[HandleError]
	public class HomeController : BaseController
	{
		[ActionName("Content")]
		public ActionResult ContentAction(string view)
		{
			try
			{
				return View(view, new ViewModel(Session));
			}
			catch
			{
				throw new HttpException(404, "Page Not Found");
			}
		}

		[HttpGet]
		public ActionResult ContactUs()
		{
			var viewModel = new ContactUsModel();
			viewModel.Session = Session;
			return View(viewModel);
		}

		[HttpPost]
		public ActionResult ContactUs(ContactUsModel model)
		{
			if (ModelState.IsValid)
			{
				MailSender.SendEmail("contact@dctua.com", model.UserEmail, "Monator's user", "User feedback for MONATOR", model.Message);
				model.Session = Session;
				return RedirectToAction("ContactUsSuccess");
			}
			model.Session = Session;
			return View(model);
		}

		[HttpGet]
		public ActionResult Faq()
		{
			return View();
		}

		public ActionResult About()
		{
			return View();
		}
	}
}
