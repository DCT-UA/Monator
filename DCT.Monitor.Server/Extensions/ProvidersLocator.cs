using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using Facebook.Session;
using System.Web.Configuration;
using Facebook.Rest;
using DCT.Monitor.Server.Extensions.Twitter;
using Facebook.Utility;
using DCT.Monitor.Server.Extensions;
using DCT.Monitor.Server.Extensions.Linkedin;
using DCT.Monitor.Server.Extensions.AuthenticationServices;
using DCT.Monitor.Modules;
using DCT.Monitor.Modules.Implementation.ProviderModule;
using LinkedIn;

namespace DCT.Monitor.Server.Extensions
{
	public static class ProvidersLocator
	{
		public static readonly TwitterInMemoryTokenManager TwitterTokenManager = new TwitterInMemoryTokenManager(Constants.TwitterConsumerKey, Constants.TwitterConsumerSecret);
		public static readonly LinkedinTokenManager LinkedinTokenManager = new LinkedinTokenManager(Constants.LinkedinApplicationKey, Constants.LinkedinApplicationSecret);
		public static readonly IProviderModule providerModule = new ProviderModule();

		public static BaseAuthenticationInfo LocateService()
		{
			BaseAuthenticationInfo authInfornation = null;
			authInfornation = LocateOpenIdResponse();
			if (authInfornation == null)
				authInfornation = LocateFacebookResponse();
			if (authInfornation == null)
				authInfornation = LocateLinkedinResponse();
			if (authInfornation == null)
				authInfornation = LocateTwitterResponse();
			return authInfornation;
		}

		private static BaseAuthenticationInfo LocateOpenIdResponse()
		{
			OpenIdRelyingParty openid = new OpenIdRelyingParty();
			var response = openid.GetResponse();
			if (response != null)
			{
				switch (response.Status)
				{
					case AuthenticationStatus.Authenticated:
						var claimsResponse = response.GetExtension<ClaimsResponse>();
						var openIdAuthInformation = new OpenIdAuthenticationInfo();
						openIdAuthInformation.Identifier = response.ClaimedIdentifier;
						if (claimsResponse != null)
						{
							openIdAuthInformation.Email = claimsResponse.Email;
							openIdAuthInformation.FirstName = claimsResponse.FullName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).First();
							if (claimsResponse.FullName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Count() > 1)
								openIdAuthInformation.LastName = claimsResponse.FullName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1];
							openIdAuthInformation.UserName = claimsResponse.Nickname;
						}

						var providerName = OpenIdProviderEndPoints.GetOpenIdProviderName(response.Provider.Uri);
						openIdAuthInformation.Provider = providerModule.GetProvider(providerName);

						return openIdAuthInformation;
				}
			}
			return null;
		}

		private static BaseAuthenticationInfo LocateFacebookResponse()
		{
			FacebookAuthenticationInfo authInformation = null;
			var connectSession = new ConnectSession(Constants.FacebookApplicationKey, Constants.FacebookApplicationSecret);
			if (connectSession.IsConnected())
			{
				var facebookApi = new Api(connectSession);
				try
				{
					var userInfo = facebookApi.Users.GetInfo();
					authInformation = new FacebookAuthenticationInfo();
					authInformation.Identifier = userInfo.uid.HasValue ? userInfo.uid.Value.ToString() : String.Empty;
					authInformation.FirstName = userInfo.first_name;
					authInformation.LastName = userInfo.last_name;
					authInformation.Email = userInfo.proxied_email;
					authInformation.UserName = userInfo.name;
					authInformation.Provider = providerModule.GetProvider("Facebook");
					authInformation.Picture = userInfo.pic;
					facebookApi.Auth.ExpireSession();
					return authInformation;
				}
				catch (FacebookException ex)
				{
					return null;
				}
			}
			return authInformation;
		}

		private static BaseAuthenticationInfo LocateTwitterResponse()
		{
			var client = new TwitterClient(TwitterTokenManager);
			var authInfo = client.FinishAuthentication();
			return authInfo;
		}

		private static BaseAuthenticationInfo LocateLinkedinResponse()
		{
			var authorization = new WebOAuthAuthorization(LinkedinTokenManager, null);
			string accessToken;
			try
			{
				accessToken = authorization.CompleteAuthorize();
			}
			catch (Exception ex)
			{
				return null;
			}
			if (accessToken != null)
			{
				authorization = new WebOAuthAuthorization(LinkedinTokenManager, accessToken);
				LinkedInService service = new LinkedInService(authorization);
				var profile = service.GetCurrentUser(ProfileType.Public);
				var info = new LinkedinAuthenticationInfo();
				info.FirstName = profile.FirstName;
				info.LastName = profile.LastName;
				info.Identifier = profile.Id;
				info.UserName = profile.Name;
				info.Provider = providerModule.GetProvider("LinkedIn");
				return info;

			}
			else
			{
				return null;
			}
		}
	}
}