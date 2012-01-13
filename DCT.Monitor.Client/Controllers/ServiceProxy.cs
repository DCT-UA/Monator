using System;
using System.Collections.Generic;
using System.Linq;
using DCT.Monitor.Client.Models;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using DCT.Unity;
using DCT.ObjectModel;

namespace DCT.Monitor.Client.Controllers
{
    public class ServiceProxy
    {
        private IDomainStatisticsDataSourceModule _dataSource;
        //private string _domain;
        private IPageSelector _site;

        private Query<DomainStatisticsItemBase> _domainStatsQuery;
        private Query<PageRequest> _domainRequestsQuery;

        private IEnumerable<DomainStatisticsItem> _domainStats;
        private IEnumerable<PageRequest> _domainRequests;

        public ServiceProxy()
        {
            _dataSource = ServiceLocator.Current.Resolve<IDomainStatisticsDataSourceModule>();

            _domainStatsQuery = new Query<DomainStatisticsItemBase>(GetDomainStatistics, TimeSpan.FromSeconds(1));
            _domainRequestsQuery = new Query<PageRequest>(GetDomainRequests, TimeSpan.FromSeconds(5));
        }

        private IEnumerable<DomainStatisticsItemBase> GetDomainStatistics()
        {
			if (App.CurrentApp.CurrentUser == null) return GenerateStats("-- logged out --");

            var domains = CreateItemsTree(_dataSource.GetDomainStatisticsByUser(App.CurrentApp.CurrentUser).Select(CopyDomainStatsToItem).ToList());
            SetMaxCount(domains);
            return domains;
        }

        private List<DomainStatisticsItemBase> CreateItemsTree(List<DomainStatisticsItemBase> raw)
        {
            var root = raw.Where(i => i.ParentId == null).ToList();
            var rootNext = root;
            var rootCurrent = root;

            for (var j = 0; j < 10 && rootNext.Count != 0; j++)
            {
                rootCurrent = rootNext;
                rootNext = new List<DomainStatisticsItemBase>();

                foreach (var element in rootCurrent)
                {
                    var data = raw.Where(i => i.ParentId == element.Id).ToList();
                    element.ChildItems = data;
                    if (data.Count == 0) element.ChildItems = null;
                    else
                    {
                        SetMaxCount(data);
                        rootNext.AddRange(data);
                    }
                }
            }

            //if (rootNext.Count != 0) throw new Exception();

            return root;
        }

        private void SetMaxCount(List<DomainStatisticsItemBase> items)
        {
            var max = items.Count == 0 ? 0 : items.Max(i => i.Count);
            items.ForEach(i => i.MaxCount = max);
        }

        private DomainStatisticsItemBase CopyDomainStatsToItem(DomainStatistics i)
        {
            var data = new DomainStatisticsDataItem();
            i.CopyTo(data);
            return data;
        }

        private IEnumerable<PageRequest> GetDomainRequests()
        {
            var data = _dataSource.GetDomainRequests(_site) ?? new PageRequest[0];
            data = data.GroupBy(i => i.SessionIdentifier).Select(RequestSessionGroup.GetEventFromGroup);

            return data;
        }

		/// <summary>
		/// It is only for displaying some strings like -- ofline --
		/// </summary>
		/// <param name="domain">string to display in tree view</param>
		/// <returns>empty DomainStatistics with domain set to parameter</returns>
		private IEnumerable<DomainStatisticsItem> GenerateStats(string domain)
		{
			return new DomainStatisticsItem[] { new DomainStatisticsItem() { Pattern = domain } };
		}

        public IEnumerable<DomainStatisticsItemBase> GetDomainStats()
        {
            return _domainStatsQuery.Data;
        }

        public IEnumerable<PageRequest> GetRequests(IPageSelector site)
        {
            _site = site;
            return _domainRequestsQuery.Data;
        }
    }
}
