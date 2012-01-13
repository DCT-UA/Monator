using System;
using System.Collections.Generic;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Modules
{
    public interface ISiteManagerModule
    {
        // creates site
        void CreateSite(User owner, Site newSite);

        // gets site by id
        Site GetSite(Guid siteId);
		Site GetSite(string domain);
		List<Site> GetSitesByUser(User user);
		void Delete(Guid siteId);
		List<Site> GetSites();
		void EditDomain(Site site);
    }
}
