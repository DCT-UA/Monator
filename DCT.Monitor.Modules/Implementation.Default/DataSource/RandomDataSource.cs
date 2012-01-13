using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inetgiant.Monitor.Entities;
using Inetgiant.Monitor.Modules;

namespace Inetgiant.Monitor.Modules.Implementation.Default.DataSource
{
    public class RandomDataSource : IDomainStatisticsDataSourceModule
    {
        private static Random _random = new Random(Environment.TickCount);

        public static RandomDataSource Source { get { return new RandomDataSource(); } }

        public IEnumerable<DomainStatistics> GetDomainStatistics()
        {
            return GetDomainList().Select(i => new DomainStatistics { Domain = i, Count = _random.Next(20), DistinctCount = _random.Next(20) });
        }

        public IEnumerable<string> GetDomainList()
        {
            return new List<string>{
                "inetgiant.com",
                "inetgiant.ca"
            };
        }

        public IEnumerable<PageRequest> GetDomainRequests(string domain)
        {
            return new PageRequest[]{
                    new PageRequest{ 
                        Domain="skyner.com", 
                        IpAddress="127.0.0.1",
                        Url="asdasd/asdasf/sdg/dsg/",
                        Browser = 1
                    }
            };
        }
    }
}
