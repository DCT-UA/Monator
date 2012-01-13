using System;
using System.Collections.Generic;
using System.Linq;
using DCT.Utils;
using DCT.Monitor.Cache;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using Monitor.DAL;
using DCT.Unity;

namespace DCT.Monitor.Modules.Implementation.DataSource
{
    public class CacheDataSource : IDomainStatisticsDataSourceModule
    {
        private static ICache _cache;
        private static IDomainStatisticsModule _domainStats;
        private static ISiteRepository _siteRepository = ServiceLocator.Current.Resolve<ISiteRepository>();
        private static IPageRepository _pageRepository = ServiceLocator.Current.Resolve<IPageRepository>();

        static CacheDataSource()
        {
            _cache = ServiceLocator.Current.Resolve<ICache>();
            _domainStats = ServiceLocator.Current.Resolve<IDomainStatisticsModule>();
        }

        private DomainStatistics GetCountOnly(IPageSelector site)
        {
            var data = new DomainStatistics(site);

            var cachedData = _domainStats.GetDomainStatistics(site);
            if(cachedData != null) data.Count = cachedData.Count;
            return data;
        }

        public List<IPageSelector> GetDomainList()
        {
            var selectorDic = _cache.Get<Dictionary<Guid, IPageSelector>>("domains") ?? new Dictionary<Guid, IPageSelector>();
            selectorDic = new Dictionary<Guid, IPageSelector>(selectorDic);

            var sites = _siteRepository.GetAll();
            var data = sites.OfType<IPageSelector>().ToList();
            var pages = _pageRepository.GetAll();
            var domains = selectorDic.Values.Where(i => i.SelectorType == (int)PageSelectorType.DomainString).ToList();
            data.AddRange(domains);

            foreach (var domain in domains)
            {
                var sitePages = _pageRepository.GetPagesBySiteId(domain.ParentId.Value);
                data.AddRange(sitePages.Select(i => new PageSelector(i, domain.Pattern))); 
            }

			return data;
        }

        public IEnumerable<PageRequest> GetDomainRequests(IPageSelector site)
        {
            return _domainStats.GetDomainRequests(site);
        }

        public IEnumerable<DomainStatistics> GetDomainStatistics()
        {
            var list = GetDomainList();

            return list == null ? null : list.Select(GetCountOnly).ToArray();
        }

		public IEnumerable<DomainStatistics> GetDomainStatisticsByUser(User user)
		{
            var fulllist = GetDomainList();
			var list = new HashSet<Guid>(fulllist.OfType<Site>().Where(i => i.UserId == user.Id).Select(i => i.Id));

            var count = 0;
            for (var index = 0; list.Count != count && index < 10; index++) // not more then 10 loops to avoid unended loop
            {
                // remember count
                count = list.Count; 

                // try to add aditional elements if they is child of already added. Count will be changed or exit loop.
                fulllist.Where(i => i.ParentId.HasValue && list.Contains(i.ParentId.Value)).Select(i => i.Id).Each(i => list.Add(i));
            }

			return list == null ? null : fulllist.Where(i => list.Contains(i.Id)).Select(GetCountOnly).ToArray();
		}
    }
}
