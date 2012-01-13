using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCT.Monitor.Entities;

namespace Monitor.DAL
{
	public interface IProviderRepository : IBaseRepository<Provider, int>
	{
		Provider GetProvider(string providerName);
	}
}
