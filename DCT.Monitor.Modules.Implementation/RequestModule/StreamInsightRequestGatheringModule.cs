using System;
using System.Collections.Generic;
using System.Linq;
using DCT.Unity;
using DCT.Monitor.Adapters;
using DCT.Monitor.Entities;
using DCT.Monitor.StreamInsight;
using DCT.Monitor.Modules;
using Monitor.DAL;

namespace DCT.Monitor.Modules.Implementation.RequestModule
{
    public class StreamInsightRequestGatheringModule : IRequestModule
    {
         private static QueriesCore _streamInsightCore;
        private static IDomainStatisticsModule _domainStatistics;
        private static IRequestRepository _requestsRepository;
        private static IConfigurationModule _config;

        static StreamInsightRequestGatheringModule()
        {
            _streamInsightCore = ServiceLocator.Current.Resolve<QueriesCore>();
            _domainStatistics = ServiceLocator.Current.Resolve<IDomainStatisticsModule>();
            _requestsRepository = ServiceLocator.Current.Resolve<IRequestRepository>();
            _config = ServiceLocator.Current.Resolve<IConfigurationModule>();

            _streamInsightCore.RegisterGathering();

            _streamInsightCore.EventsOutputService.Next += DomainRequestsHandler;
            if (_config.SaveRequestsToDb)
            {
                _streamInsightCore.EventsOutputService.Next += StatInput;
            }
        }

        private static void StatInput(object sender, EventArgs<IEnumerable<PageRequest>> data)
        {
            IEnumerable<PageRequest> data2 = data.Data.Where(t => t.Duration == TimeSpan.Zero);
            _requestsRepository.Create(data2);
        }

        private static void DomainStatisticsHandler(object sender, EventArgs<DomainStatistics> e)
        {
            try
            {
                var data = e.Data;
                //ServiceLocator.LoggerService.Info(string.Format("Id:{0}, Count:{1}, Date: {2}",data.Id,data.Count, DateTime.Now.TimeOfDay));
                _domainStatistics.SetDomainStatistics(e.Data, new DomainStatisticsData(e.Data));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private static void DomainRequestsHandler(object sender, EventArgs<IEnumerable<PageRequest>> data)
        {
            foreach (var i in data.Data.ToLookup(i => i.SiteId, i => i))
            {
				PutList(new Site { Id = i.Key }, i);
            }

            foreach (var i in data.Data.ToLookup(i => i.Domain, i => i))
            {
                var siteId = i.First().SiteId;
				PutList(new DomainSelector(i.Key, siteId), i);
            }
            
            foreach (var i in data.Data.ToLookup(i => i.PageId, i => i))
            {
                if (!i.Key.HasValue) continue;
                var first = i.First();
                var page = new Page { Id = i.Key.Value, DomainId = first.SiteId };

                PutList(new PageSelector(page, "total"), i);
            }

            foreach (var i in data.Data.ToLookup(i => new { i.PageId, i.Domain }, i => i))
            {
                if (!i.Key.PageId.HasValue) continue;
                var first = i.First();
                var page = new Page { Id = i.Key.PageId.Value, DomainId = first.SiteId };

                PutList(new PageSelector(page, i.Key.Domain), i);
            }
        }

        private static bool PutList(IPageSelector site, IEnumerable<PageRequest> list)
        {
            if (site == null) return false;
            _domainStatistics.SetDomainRequests(site, list);
            return true;
        }

        public void SendRequest(PageRequest request)
        {
            RequestHelper.ProcessRequest(request);

            _streamInsightCore.PingInputServiceGathering.PushEvent(request);
        }
    }
}
