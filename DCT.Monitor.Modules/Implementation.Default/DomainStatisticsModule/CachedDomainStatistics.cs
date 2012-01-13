using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inetgiant.Monitor.Cache;
using My.Unity;
using Inetgiant.Monitor.Entities;

namespace Inetgiant.Monitor.Modules.Implementation.Default.DomainStatisticsModule
{
    public class CachedDomainStatistics: IDomainStatisticsModule
    {
        private static ICache Cache;

        static CachedDomainStatistics()
        {
            Cache = ServiceLocator.Current.Resolve<ICache>();
        }

        public void SetDomainStatistics(DomainStatistics domainStatistics)
        {
            Cache.Put(domainStatistics.Domain, domainStatistics, DateTime.Now.AddSeconds(15));

            // hack for domain list
            var list = Cache.Get<List<string>>("domains") ?? new List<string>();
            if (!list.Contains(domainStatistics.Domain))
            {
                list.Add(domainStatistics.Domain);
                Cache.Put("domains", list);
            }
        }

        public void SetDomainRequests(string domain, IEnumerable<PageRequest> events)
        {
            Cache.Put(domain + "/requests", events.ToArray(), DateTime.Now.AddSeconds(15));
        }

        public DomainStatistics GetDomainStatistics(string domain)
        {
            return Cache.Get<DomainStatistics>(domain);
        }

        public IEnumerable<PageRequest> GetDomainRequests(string domain)
        {
            return Cache.Get<IEnumerable<PageRequest>>(domain + "/requests");
        }
    }
}
