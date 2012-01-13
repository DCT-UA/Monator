using System;
using System.Collections.Generic;
using System.Linq;
using DCT.Unity;
using System.Collections.Concurrent;
using DCT.Monitor.Cache;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;

namespace DCT.Monitor.Modules.Implementation.DomainStatisticsModule
{
    public class CachedDomainStatistics: IDomainStatisticsModule
    {
        private static ICache Cache;

        private static ConcurrentDictionary<Guid,DateTime> _activeRequests = new ConcurrentDictionary<Guid,DateTime>();
        private static IConfigurationModule _config = ServiceLocator.Current.Resolve<IConfigurationModule>();

        static CachedDomainStatistics()
        {
            Cache = ServiceLocator.Current.Resolve<ICache>();
        }

        public void SetDomainStatistics(IPageSelector selector, DomainStatisticsData domainStatistics)
        {
            Cache.Put(selector.Id.ToString(), domainStatistics, DateTime.Now.AddSeconds(250));

            // hack for domain list
            var list = Cache.Get<Dictionary<Guid, IPageSelector>>("domains") ?? new Dictionary<Guid, IPageSelector>();
            if (!list.ContainsKey(selector.Id))
            {
                list.Add(selector.Id, selector);
                Cache.Put("domains", list);
            }
        }

        public void SetDomainRequests(IPageSelector selector, IEnumerable<PageRequest> events)
        {
            Cache.Put(selector.Id.ToString() + "/requests", events.ToArray(), DateTime.Now.AddSeconds(250));
        }

		public DomainStatisticsData GetDomainStatistics(IPageSelector selector)
        {
            return Cache.Get<DomainStatisticsData>(selector.Id.ToString());
        }

		public IEnumerable<PageRequest> GetDomainRequests(IPageSelector selector)
        {
            var time = DateTime.Now.AddSeconds(_config.CoreDelay * 2);
            _activeRequests.AddOrUpdate(selector.Id, time, ( guid, date) => time );

            return Cache.Get<IEnumerable<PageRequest>>(selector.Id.ToString() + "/requests");
        }

        public bool Contains(IPageSelector selector) 
        {
            DateTime time;
            return _activeRequests.TryGetValue(selector.Id, out time) && time >= DateTime.Now;
        }

    }
}
