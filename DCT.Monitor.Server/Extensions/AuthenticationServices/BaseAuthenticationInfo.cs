using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DCT.Monitor.Server.Models;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Server.Extensions.AuthenticationServices
{
	public abstract class BaseAuthenticationInfo
	{
		public abstract string Identifier { get; set; }
		public string UserName { get; set; }
		public Provider Provider { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}