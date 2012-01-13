using System.Collections.Generic;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;

namespace DCT.Monitor.ServiceDataSource
{
    public class JsonDataSource : IDomainStatisticsDataSourceModule
    {
        private JsonDataServiceClient _client = new JsonDataServiceClient();

        public IEnumerable<DomainStatistics> GetDomainStatistics()
        {
            return _client.GetDomainsStats();
        }

        public IEnumerable<DomainStatistics> GetDomainStatisticsByUser(User user)
        {
            return GetDomainStatistics();
        }

        public IEnumerable<PageRequest> GetDomainRequests(IPageSelector selector)
        {
            return _client.GetDomainRequests(selector);
        }
    }
}
