using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;
using DCT.Monitor.Server.Extensions.AuthenticationServices;
using DotNetOpenAuth.OAuth.Messages;
using System.Xml;
using System.Web.Configuration;
using DCT.Monitor.Modules;
using DCT.Monitor.Modules.Implementation.ProviderModule;

namespace DCT.Monitor.Server.Extensions.Twitter
{
	public class TwitterClient
	{
		private static readonly ServiceProviderDescription ServiceDescription =
			new ServiceProviderDescription
			{
				RequestTokenEndpoint = new MessageReceivingEndpoint(
										   TwitterEndpoints.RequestTokenEndpoint,
										   HttpDeliveryMethods.GetRequest |
										   HttpDeliveryMethods.AuthorizationHeaderRequest),

				UserAuthorizationEndpoint = new MessageReceivingEndpoint(
										  TwitterEndpoints.UserAuthorizationEndpoint,
										  HttpDeliveryMethods.GetRequest |
										  HttpDeliveryMethods.AuthorizationHeaderRequest),

				AccessTokenEndpoint = new MessageReceivingEndpoint(
										  TwitterEndpoints.AccessTokenEndpoint,
										  HttpDeliveryMethods.GetRequest |
										  HttpDeliveryMethods.AuthorizationHeaderRequest),

				TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
			};
		public static readonly IProviderModule providerModule = new ProviderModule();

		IConsumerTokenManager _tokenManager;

		public TwitterClient(IConsumerTokenManager tokenManager)
		{
			_tokenManager = tokenManager;
		}

		public void StartAuthentication(Uri callBackUrl)
		{
			var request = HttpContext.Current.Request;
			using (var twitter = new WebConsumer(ServiceDescription, _tokenManager))
			{
				twitter.Channel.Send(
					twitter.PrepareRequestUserAuthorization(callBackUrl, null, null)
				);
			}
		}

		public TwitterAuthenticationInfo FinishAuthentication()
		{
			TwitterAuthenticationInfo authInfo = null;
			using (var twitter = new WebConsumer(ServiceDescription, _tokenManager))
			{
				var accessTokenResponse = twitter.ProcessUserAuthorization();
				if (accessTokenResponse != null)
				{
					authInfo = new TwitterAuthenticationInfo();
					authInfo.Provider = providerModule.GetProvider("Twitter");/*context.Providers.Where(provider => provider.Name == "Twitter").Single();*/
					authInfo.UserName = accessTokenResponse.ExtraData["screen_name"];
					authInfo.Identifier = accessTokenResponse.ExtraData["user_id"];
					PopulateTwitterInfo(accessTokenResponse, authInfo);
					return authInfo;
				}
			}

			return authInfo;
		}

		private void PopulateTwitterInfo(AuthorizedTokenResponse response, TwitterAuthenticationInfo info)
		{
			string url = String.Format("http://api.twitter.com/1/users/show.xml?user_id={0}", info.Identifier);
			XmlDocument document = new XmlDocument();
			document.Load(url);
			XmlNode userNode = document.DocumentElement.SelectSingleNode("//user");
			string[] firstnameAndLastname = userNode["name"].InnerText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
			if (firstnameAndLastname.Length > 1)
			{
				info.FirstName = firstnameAndLastname[0];
				info.LastName = firstnameAndLastname[1];
			}
		}

	}
}