using System;
using System.Collections.Generic;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using Monitor.DAL;
using DCT.Unity;

namespace DCT.Monitor.Modules.Implementation.SiteManager
{
    public class SiteManagerModule: ISiteManagerModule
    {
        private ISiteRepository _repository = ServiceLocator.Current.Resolve<ISiteRepository>();

        public void CreateSite(User owner, Site newSite)
        {
            newSite.UserId = owner.Id;
			newSite.Domain = newSite.Domain.Replace("www.", string.Empty);
            _repository.Create(newSite);
        }

		//Get site by site ID
        public Site GetSite(Guid siteId)
        {
            return _repository.GetById(siteId);
        }

		public Site GetSite(string domain)
		{
			return _repository.GetSite(domain);
		}

		//Get user sites list
		public List<Site> GetSitesByUser(User user)
		{
			return _repository.GetSitesByUser(user.Id);
		}

		//Delete site
		public void Delete(Guid siteId)
		{
			_repository.Delete(siteId);
		}

		public List<Site> GetSites()
		{
			return _repository.GetSites();
		}

		public void EditDomain(Site site)
		{
			_repository.Update(site);
		}
    }
}
