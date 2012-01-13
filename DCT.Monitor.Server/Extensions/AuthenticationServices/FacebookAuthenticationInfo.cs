using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCT.Monitor.Server.Extensions.AuthenticationServices
{
	public class FacebookAuthenticationInfo : BaseAuthenticationInfo
	{
		public override string Identifier { get; set; }
		public string Picture { get; set; }
	}
}