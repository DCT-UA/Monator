using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCT.Monitor.Server.Extensions.AuthenticationServices
{
	public class OpenIdAuthenticationInfo : BaseAuthenticationInfo
	{
		public override string Identifier { get; set; }
		public ProviderInfo ProviderInformation { get; set; }
	}
}