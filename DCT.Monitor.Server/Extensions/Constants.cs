using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace DCT.Monitor.Server.Extensions
{
	public static class Constants
	{
		public const string IssuerName = "IssuerName";
		public const string SigningCertificateName = "SigningCertificateName";
		public const string EncryptingCertificateName = "EncryptingCertificateName";
		public const string ProviderSessionKey = "ProviderSessionKey";
		public const string COUNTRIES_CONFIG_KEY = "countriesConfig";
		public const string COUNTRIES_SECTION_NAME = "CountriesConfiguration";
		public const string COUNTRY_INSTANCE = "currentCountry";
		public const string FacebookLogoffSessionKey = "FacebookLogoff";

		public static string TwitterConsumerKey
		{
			get
			{
				return WebConfigurationManager.AppSettings["TwitterConsumerKey"];
			}
		}
		public static string TwitterConsumerSecret
		{
			get
			{
				return WebConfigurationManager.AppSettings["TwitterConsumerSecret"];
			}
		}
		public static string FacebookApplicationKey
		{
			get
			{
				return WebConfigurationManager.AppSettings["FacebookApplicationKey"];
			}
		}
		public static string FacebookApplicationSecret
		{
			get
			{
				return WebConfigurationManager.AppSettings["FacebookSecretKey"];
			}
		}
		public static string LinkedinApplicationKey
		{
			get
			{
				return WebConfigurationManager.AppSettings["LinkedinApplicationKey"];
			}
		}
		public static string LinkedinApplicationSecret
		{
			get
			{
				return WebConfigurationManager.AppSettings["LinkedinSecretKey"];
			}
		}
		public static string NotificationPassword
		{
			get
			{
				return WebConfigurationManager.AppSettings["NotificationPassword"];
			}
		}
	}

	public struct TwitterEndpoints
	{
		public static string RequestTokenEndpoint = "http://twitter.com/oauth/request_token";
		public static string UserAuthorizationEndpoint = "http://twitter.com/oauth/authorize";
		public static string AccessTokenEndpoint = "http://twitter.com/oauth/access_token";
	}
	public struct OpenIdProviderEndPoints
	{
		public static ProviderInfo Google = new ProviderInfo { EndPoint = "https://www.google.com/accounts/o8/id", Image = "google.png" };
		public static ProviderInfo Yahoo = new ProviderInfo { EndPoint = "https://me.yahoo.com", Image = "yahoo.png" };
		public static ProviderInfo Flickr = new ProviderInfo { EndPoint = "http://www.flickr.com", Image = "flickr.png" };
		public static ProviderInfo MySpace = new ProviderInfo { EndPoint = "http://www.myspace.com", Image = "myspace.png" };
		public static ProviderInfo Aol = new ProviderInfo { EndPoint = "http://openid.aol.com", Image = "aol.png" };
		public static ProviderInfo LiveJournal = new ProviderInfo { EndPoint = "http://username.livejournal.com", Image = "livejournal.png" };
		public static ProviderInfo MyOpenId = new ProviderInfo { EndPoint = "http://username.myopenid.com", Image = "myopenid.png" };
		public static ProviderInfo OpenId = new ProviderInfo { EndPoint = String.Empty, Image = "openidBig.png" };
		public static ProviderInfo WindowsLiveId = new ProviderInfo { EndPoint = "http://openid.live-int.com", Image = String.Empty };
		public static ProviderInfo VKontakte = new ProviderInfo { EndPoint = "http://VKontakteID.ru", Image = "vkontakte.png" };
		//public static ProviderInfo Linkedin = new ProviderInfo { EndPoint = "https://api.linkedin.com", Image = "linkedin.png" };

		public static string GetOpenIdProviderName(Uri endPointUri)
		{
			if (endPointUri.Host == new Uri(VKontakte.EndPoint).Host)
				return "VKontakte";
			if (endPointUri.Host == "api.myspace.com")
				return "MySpace";
			return "OpenId";
		}
	}

    public class MailSenderConstants
    {
        public static string EMAIL_SENDER_ADDRESS = WebConfigurationManager.AppSettings["emailSenderAddress"];
        public static string EMAIL_SENDER_NAME = WebConfigurationManager.AppSettings["passwordRecoverysSenderName"];
        public static string EMAIL_PASSWORD_RECOVERY_SUBJECT = WebConfigurationManager.AppSettings["passwordRecoverySubject"];
        public static string PASSWORD_RECOVERY_EMAIL_TEMPLATE = "Dear {0}, \nyou have used a change password service.  To continue using Monator, please, login in with your new password. \nYour new password is:{1}\nYours faithfully, Monator’s administration.";
    }

	public class ProviderInfo
	{
		public string EndPoint { get; set; }
		public string Image { get; set; }
	}
}