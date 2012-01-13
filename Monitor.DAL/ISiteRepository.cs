using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCT.Monitor.Entities;

namespace Monitor.DAL
{
    public interface ISiteRepository : IBaseRepository<Site, Guid>
    {
		List<Site> GetSitesByUser(Guid userId);
		Site GetSite(string domain);
		List<Site> GetSites();
    }
}
