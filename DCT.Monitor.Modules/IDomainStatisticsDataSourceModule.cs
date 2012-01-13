using System.Collections.Generic;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Modules
{
    public interface IDomainStatisticsDataSourceModule
    {
        IEnumerable<DomainStatistics> GetDomainStatistics();
		IEnumerable<DomainStatistics> GetDomainStatisticsByUser(User user);
        IEnumerable<PageRequest> GetDomainRequests(IPageSelector selector);
    }
}
