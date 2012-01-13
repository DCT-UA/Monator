using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using DCT.Monitor.Server.Models;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.RelyingParty;
using DCT.Monitor.Server.Extensions;
using DCT.Monitor.Server.Extensions.Twitter;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SqlTypes;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using DCT.Monitor.Modules.Implementation.ProviderModule;
using DCT.Monitor.Modules.Implementation.UserModule;
using LinkedIn;
using DCT.Monitor.Server.Code;
using DCT.Monitor.Server.Helpers;
using DCT.Utils;
using DCT.Monitor.Server.Security;

namespace DCT.Monitor.Server.Controllers
{

	[HandleError]
	public class AccountController : BaseController
	{
		public IFormsAuthenticationService FormsService { get; set; }
		public IAccountService AccountService { get; set; }

		private IUserModule userModule = new UserModule();
		private IProviderModule providerModule = new ProviderModule();
        
		private User userBind = new User();

		protected override void Initialize(RequestContext requestContext)
		{
			if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
			if (AccountService == null) { AccountService = new AccountService(); }

			base.Initialize(requestContext);
		}

		[HttpGet]
		public ActionResult LogOn()
		{
			HttpCookie cookieProvider = Request.Cookies.Get("SkynerCookie");
			if (cookieProvider == null)
			{
				ViewBag.SiteName = "Undefined";
			}
			else
			{
				ViewBag.SiteName = cookieProvider.Value;
			}

            ViewData.Model = new AccountModel(new LogOnModel());
			return View();
		}

        public ActionResult LogOnUsingOpenId(string site)
        {
			var locatedUser = ProvidersLocator.LocateService();
			if (locatedUser != null)
			{
				var id = locatedUser.Identifier;
				if (!String.IsNullOrEmpty(id))
				{
					Session.User = new User();
					Session.User.ProviderUserId = id;
					Session.User.ProviderId = (int)locatedUser.Provider.Id;
					var identifier = userModule.GetUserByProviderUserId(id);
					if (identifier != null)
					{
						Session.User.Id = identifier.Id;
						Session.User = userModule.GetUser(identifier.UserName);

						ViewData["UserName"] = Session.User.UserName;
						FormsService.SignIn(identifier.UserName, false);
						Session.SessionEndTime = DateTime.Now;
						return Redirect(Url.Home());
					}
					else
					{
						ViewData["ProviderId"] = id;
						return Redirect(Url.Register(), true);
					}
				}
			}

            switch (site)
            {
                case "twitter":
                    var client = new TwitterClient(ProvidersLocator.TwitterTokenManager);
                    client.StartAuthentication(Request.Url);
                    break;
                case "google":
                    TransferToOpenIdProvider(OpenIdProviderEndPoints.Google.EndPoint);
                    break;
                case "vkontakte":
                    TransferToOpenIdProvider(OpenIdProviderEndPoints.VKontakte.EndPoint);
                    break;
                case "linkedin":
                    var authorization = new WebOAuthAuthorization(ProvidersLocator.LinkedinTokenManager, null);
                    var callback = Request.Url;
                    authorization.BeginAuthorize(callback);
                    break;
                case "myspace":
                    TransferToOpenIdProvider(OpenIdProviderEndPoints.MySpace.EndPoint);
                    break;
            }

            ViewData.Model = new AccountModel(new LogOnModel());
            return View("LogOn");
        }

		[HttpPost]
		public ActionResult LogOn(AccountModel model)
		{
			if (!ModelState.IsValid)
			{
				string scriptId = "signin";
				ViewData.Model = scriptId;
				ViewData.Model = new AccountModel(new LogOnModel());
				return View();
			}

            ViewData.Model = new AccountModel(model.LogonModel);

            var userValid = userModule.Authenticate(new User() { UserName = model.LogonModel.UserName, Password = model.LogonModel.Password });
            if (userValid)
            {
                Session.Refresh(HttpContext);
                HttpCookie providerCookie = new HttpCookie("SkynerCookie");

                var provider = providerModule.GetProvider(Session.User.ProviderId);
                if(provider != null) providerCookie.Value = provider.Name;
                Response.Cookies.Add(providerCookie);

                ViewData["UserName"] = Session.User.UserName;
                FormsService.SignIn(model.LogonModel.UserName, model.LogonModel.RememberMe);
				Session.SessionEndTime = DateTime.Now;
                return Redirect(Url.Home());
            }

			return View();
		}

		public ActionResult LogOff()
		{
			Session.User = null;
			FormsService.SignOut();

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ActionResult Register()
		{
			string scriptId = "signup";
			ViewData.Model = scriptId;
			ViewData["ProviderId"] = Session.User.ProviderId;
			ViewData["ProviderUserId"] = Session.User.ProviderUserId;
            ViewData["PasswordLength"] = AccountService.MinPasswordLength;

            ViewData.Model = new AccountModel( new RegisterModel() );

			return View("LogOn");
		}

		[HttpPost]
		public ActionResult Register(RegisterModelContainer model, string site)
		{
			var user = new User()
			{
				UserName = model.RegisterModel.UserName,
				Password = model.RegisterModel.Password.HashString(),
				Email = model.RegisterModel.Email,
				ProviderId = 1,
				ProviderUserId = ""
			};
				
            user.ProviderId = Session.User.ProviderId;
			user.ProviderUserId = Session.User.ProviderUserId;

			Session.User = user;

            ViewData.Model = model.RegisterModel;
			TryValidateModel(model.RegisterModel);
			var isValid = ModelState.IsValid;
			ViewData.Model = model;
			if (isValid)
			{
				Guid g = Guid.NewGuid();
				user.Id = g;
						
				if (user.ProviderUserId == null)
				{
					user.ProviderId = 1;
					user.ProviderUserId = string.Empty;
				}

				userModule.Create(user);
				HttpCookie providerCookie = new HttpCookie("SkynerCookie");

				Session.User = user;
				providerCookie.Value = "Undefined";
				Response.Cookies.Add(providerCookie);

				ViewData["UserName"] = Session.User.UserName;
				FormsService.SignIn(user.UserName, false); 
				Session.SessionEndTime = DateTime.Now;
				return RedirectToAction("AccountBinding", "Account");
			}
			string scriptId = "signup";
			ViewData.Model = scriptId;
			ViewData.Model = new AccountModel(new RegisterModel());
			return View("LogOn" );
		}

        [HttpGet]
        public ActionResult ForgotPassword()
        {
			ViewData["ProviderId"] = Session.User.ProviderId;
			ViewData["ProviderUserId"] = Session.User.ProviderUserId;

            return View();
        }

		[HttpPost]
		public ActionResult ForgotPassword(ForgotPasswordModel model)
		{
			if (ModelState.IsValid)
			{
				var userEmail = userModule.GetEmail(model.UserName);
				var userId = userModule.GetId(model.UserName);
				var newPassword = userModule.GeneratePassword();
				var hashPassword = newPassword.HashString();

				userModule.RestorePassword(hashPassword, userId);
                MailSender.SendPasswordRecoveryEmail(model.UserName, userEmail, newPassword);
				return RedirectToAction("ForgotPasswordSuccess");
			}
			else
			{
				return View();
			}
		}

		[Authorize, HttpGet]
		public ActionResult AccountBinding()
		{
			ViewData["ProviderId"] = Session.User.ProviderId;
			if (ViewData.Model == null)
			{
				ViewData.Model = null;
			}

			var locatedUser = ProvidersLocator.LocateService();
			if (locatedUser != null)
			{
				var user = Session.User as User;
				var id = locatedUser.Identifier;

				userBind = userModule.GetUserByProviderUserId(id);
				if (userBind != null)
					ViewData.Model = userBind;
				else
					ViewData.Model = 1;

				var providerName = locatedUser.Provider.Name;
				ViewData["ProviderUserId"] = id;

				var providerType = providerModule.GetProvider(providerName).Id;
				ViewData["ProviderId"] = providerType;
				var model = new BindingModel(new ChangeBindingModel(user, userBind));
				model.AccountBinding.ProviderId = providerType;
				model.AccountBinding.ProviderUserId = id;
				model.Session = Session;
				return View(model);
			}
		var viewModel = new BindingModel(new ChangeBindingModel());
			viewModel.Session = Session;
			return View(viewModel);
		}

		[Authorize]
		[HttpPost]
		public ActionResult AccountBinding(BindingModel model, string site)
		{
			switch (site)
			{
				case "twitter":
					var client = new TwitterClient(ProvidersLocator.TwitterTokenManager);
					client.StartAuthentication(Request.Url);
					break;
				case "google":
					TransferToOpenIdProvider(OpenIdProviderEndPoints.Google.EndPoint);
					break;
				case "vkontakte":
					TransferToOpenIdProvider(OpenIdProviderEndPoints.VKontakte.EndPoint);
					break;
				case "linkedin":
					var authorization = new WebOAuthAuthorization(ProvidersLocator.LinkedinTokenManager, null);
					var callback = Request.Url;
					authorization.BeginAuthorize(callback);
					break;
				case "myspace":
					TransferToOpenIdProvider(OpenIdProviderEndPoints.MySpace.EndPoint);
					break;
				default:
					int curProviderId;
					string curProviderUserId;

					userBind = userModule.GetUserByProviderUserId(model.AccountBinding.ProviderUserId);
					if (userBind != null)
						userModule.CleanProvider(model.AccountBinding.ProviderUserId);
					curProviderId = model.AccountBinding.ProviderId;
					curProviderUserId = model.AccountBinding.ProviderUserId;

					if (userModule.AddProvider(curProviderId, curProviderUserId, Session.User.Id))
					{
						HttpCookie providerCookie = new HttpCookie("SkynerCookie");

						Session.User.ProviderId = curProviderId;
						Session.User.ProviderUserId = curProviderUserId;
						
						providerCookie.Value = providerModule.GetProvider(Session.User.ProviderId).Name;
						Response.Cookies.Add(providerCookie);

						ViewData["UserName"] = Session.User.UserName;
						ViewData.Model = null;
						return RedirectToAction("AccountBindingSuccess", "Account");
					}
					break;
			}
			model.Session = Session;
			return View();
		}

		private void TransferToOpenIdProvider(string endPoint)
		{
			try
			{
				using (OpenIdRelyingParty openid = new OpenIdRelyingParty())
				{
					IAuthenticationRequest request = openid.CreateRequest(endPoint);
					request.AddExtension(new ClaimsRequest
					{
						Email = DemandLevel.Require,
						FullName = DemandLevel.Require
					});
					request.RedirectToProvider();
				}
			}
			catch (ProtocolException ex)
			{
			}
		}


		[Authorize]
		public ActionResult ChangePassword()
		{
			if (Session.User != null)
			{
				ViewData["PasswordLength"] = AccountService.MinPasswordLength;
				var viewModel = new ViewModel();
				viewModel.Session = Session;
				return View();
			}
			else
			{
				return Redirect(Url.Logon(), true);
			}
		}

		[Authorize]
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			if (Session.User != null)
			{
				if (ModelState.IsValid)
					if (userModule.ChangePassword(model.OldPassword.HashString(), model.NewPassword.HashString(), Session.User.Id))
						return RedirectToAction("ChangePasswordSuccess");
					else
						ModelState.AddModelError("", "Current password is not valid! Please enter a valid current password.");

				// If we got this far, something failed, redisplay form
				ViewData["PasswordLength"] = AccountService.MinPasswordLength;
				
				model.Session = Session;
				return View(model);
			}
			else
			{
				return Redirect(Url.Logon(), true);
			}
		}

		[Authorize]
		[HttpGet]
		public ActionResult MyHome()
		{
			return Redirect(Url.Sites());
		}

		public ActionResult ChangePasswordSuccess()
		{
			var viewModel = new ChangePasswordModel();
			viewModel.Session = Session;
			return View(viewModel);
		}

		public ActionResult ForgotPasswordSuccess()
		{
			var viewModel = new ViewModel();
			viewModel.Session = Session;
			return View();
		}

		public ActionResult AccountBindingSuccess()
		{
			var viewModel = new BindingModel();
			viewModel.Session = Session;
			return View(viewModel);
		}

		public ActionResult Facebook()
		{
			var viewModel = new ChangePasswordModel();
			viewModel.Session = Session;
			return View(viewModel);
		}

	}
}
