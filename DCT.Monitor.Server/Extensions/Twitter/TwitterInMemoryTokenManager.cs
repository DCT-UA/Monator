using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OpenId.Extensions.OAuth;
using DotNetOpenAuth.OAuth.Messages;

namespace DCT.Monitor.Server.Extensions.Twitter
{
	public class TwitterInMemoryTokenManager : IConsumerTokenManager, IOpenIdOAuthTokenManager
	{
		private Dictionary<string, string> tokensAndSecrets =
	   new Dictionary<string, string>();

		public TwitterInMemoryTokenManager(string consumerKey, string consumerSecret)
		{
			if (String.IsNullOrEmpty(consumerKey))
			{
				throw new ArgumentNullException("consumerKey");
			}

			this.ConsumerKey = consumerKey;
			this.ConsumerSecret = consumerSecret;
		}

		public string ConsumerKey { get; private set; }
		public string ConsumerSecret { get; private set; }

		public string GetTokenSecret(string token)
		{
			return this.tokensAndSecrets[token];
		}

		public void StoreNewRequestToken(UnauthorizedTokenRequest request,
										ITokenSecretContainingMessage response)
		{
			this.tokensAndSecrets[response.Token] = response.TokenSecret;
		}

		public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey,
			string requestToken, string accessToken, string accessTokenSecret)
		{
			this.tokensAndSecrets.Remove(requestToken);
			this.tokensAndSecrets[accessToken] = accessTokenSecret;
		}

		public TokenType GetTokenType(string token)
		{
			throw new NotImplementedException();
		}

		public void StoreOpenIdAuthorizedRequestToken(string consumerKey,
			AuthorizationApprovedResponse authorization)
		{
			this.tokensAndSecrets[authorization.RequestToken] = String.Empty;
		}

	}
}