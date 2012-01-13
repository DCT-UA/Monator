using System.Collections.Generic;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Modules
{
    public interface IDomainStatisticsModule
    {
        void SetDomainStatistics(IPageSelector selector, DomainStatisticsData domainStatistics);
        void SetDomainRequests(IPageSelector selector, IEnumerable<PageRequest> requests);
        DomainStatisticsData GetDomainStatistics(IPageSelector selector);
        IEnumerable<PageRequest> GetDomainRequests(IPageSelector selector);
        bool Contains(IPageSelector selector);
    }
}
