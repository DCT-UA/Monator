using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Inetgiant.Monitor.Cache;
using Inetgiant.Monitor.Entities;
using Inetgiant.Monitor.Modules;
using My.Unity;

namespace Inetgiant.Monitor.Modules.Implementation.Default.DataSource
{
    public class CacheDataSource : IDomainStatisticsDataSourceModule
    {
        private static ICache _cache;
        private static IDomainStatisticsModule _domainStats;

        static CacheDataSource()
        {
            _cache = ServiceLocator.Current.Resolve<ICache>();
            _domainStats = ServiceLocator.Current.Resolve<IDomainStatisticsModule>();
        }

        private DomainStatistics GetCountOnly(string domain)
        {
            var stats = _domainStats.GetDomainStatistics(domain);
  
            return
                new DomainStatistics
                {
                    Domain = domain,
                    Count = stats == null ? 0 : stats.Count,
                    DistinctCount = stats == null ? 0 : stats.DistinctCount
                };
        }

        public IEnumerable<string> GetDomainList()
        {
            return _cache.Get<List<string>>("domains");
        }

        public IEnumerable<PageRequest> GetDomainRequests(string domain)
        {
            return _domainStats.GetDomainRequests(domain);
        }

        public IEnumerable<DomainStatistics> GetDomainStatistics()
        {
            var list = GetDomainList();

            return list == null ? null : list.Select(GetCountOnly).Where(i => i.Count != 0).ToArray();
        }
    }
}
