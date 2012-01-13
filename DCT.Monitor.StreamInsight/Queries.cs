using System;
using System.Collections.Generic;
using DCT.Monitor.Adapters;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Linq;
using DCT.Unity;
using System.ServiceModel;
using Microsoft.ComplexEventProcessing.ManagementService;

namespace DCT.Monitor.StreamInsight
{
    public class QueriesCore : IDisposable
    {
        private Server _cepServer;
        private Application _cepApplication;

        private InputCollection<PageRequest> _pingSource;
        private InputCollection<PageRequest> _pingSourceGathering;
        private ICepObservable<DomainStatistics> _domainStatSource;
        private ICepObservable<PointEvent<PageRequest>> _pingOutputSource;

        private ICepObservable<PointEvent<PageRequest>> _statisticOutputSource;

        private CepStream<PageRequest> _pingInputStream;
        private CepStream<PageRequest> _pingInputStreamGathering;
        private CepObserver<DomainStatistics> _statsOutputService;
        private CepDataCollector<PageRequest> _statisticOutputService;
        private CepDataCollector<PageRequest> _eventsOutputService;

        private bool _disposed;
        private IDisposable _subscription;
        private IDisposable _subscription2;
        private IDisposable _StatisticSubscription;
        private bool _distinctNeeded;

        public IInputService<PageRequest> PingInputService { get { return _pingSource; } }
        public IInputService<PageRequest> PingInputServiceGathering { get { return _pingSourceGathering; } }
        public IOutputService<DomainStatistics> DomainStatisticsOutputService { get { return _statsOutputService; } }
        public IOutputService<IEnumerable<PageRequest>> EventsOutputService { get { return _eventsOutputService; } }
        public IOutputService<IEnumerable<PageRequest>> EventsStatistics { get { return _statisticOutputService; } }
        private static readonly IConfigurationModule config = ServiceLocator.Current.Resolve<IConfigurationModule>();
        private ServiceHost _cepManagementHost;

        public QueriesCore(string instance, bool distinctNeeded)
        {
            // TODO: Complete member initialization
            this._distinctNeeded = distinctNeeded;
             _cepServer = Server.Create(instance);
            _cepApplication = _cepServer.CreateApplication("Monitor");

            _pingSource = new InputCollection<PageRequest>();
            _pingSourceGathering = new InputCollection<PageRequest>();

            _pingInputStream = _pingSource.ToPointStream(
                _cepApplication, 
                s => PointEvent.CreateInsert(DateTime.UtcNow, s), 
                AdvanceTimeSettings.IncreasingStartTime, 
                "PagePingStream");

            _pingInputStreamGathering = _pingSourceGathering.ToPointStream(
                _cepApplication,
                s => PointEvent.CreateInsert(DateTime.UtcNow, s),
                AdvanceTimeSettings.IncreasingStartTime,
                "PagePingStream");

        }

        public IManagementService GetManagementService()
        {
            return _cepServer.CreateManagementService();
        }

        private void RegisterManagementService(Server cepServer, string hostUrl)
        {
            /////////////////////////////////////////////////////////
            // Register the management service
            var service = cepServer.CreateManagementService();
            _cepManagementHost = new ServiceHost(service);

            _cepManagementHost.AddServiceEndpoint(
                typeof(IManagementService),
                new WSHttpBinding(SecurityMode.Message),
                hostUrl);

            _cepManagementHost.Open();
        }

        public void RegisterGathering() 
        {
            int timeout = config.CoreDelay;

            var outStream2 = (from window in _pingInputStreamGathering.HoppingWindow(
                                 TimeSpan.FromSeconds(timeout),
                                 TimeSpan.FromSeconds(timeout),
                                 HoppingWindowOutputPolicy.ClipToWindowEnd)
                              from e in window
                              orderby e.Duration
                              select e);
            _pingOutputSource = outStream2.Take(int.MaxValue).ToPointObservable();

            try
            {
                _subscription2 = _pingOutputSource.Subscribe(_eventsOutputService);
            }
            catch (Exception e)
            {
                e.ToString();
                throw;
            }
        }


        public void RegisterCounting()
        {
            int timeout = config.CoreDelay;               


            var domainStats =
                from e in _pingInputStream
                group e by new { e.ParentDomain, e.SiteId } into domainGroups
                from win in domainGroups.HoppingWindow(
                    TimeSpan.FromSeconds(timeout),
                    TimeSpan.FromSeconds(timeout / 5),
                    HoppingWindowOutputPolicy.ClipToWindowEnd)
                select new DomainStatistics
                {
                    ParentId = domainGroups.Key.SiteId,
                    Id = DomainSelector.GetDomainId(domainGroups.Key.ParentDomain, domainGroups.Key.SiteId),
                    SelectorType = (int)PageSelectorType.DomainString,
                    Count = (int)win.Count(),
                    Pattern = domainGroups.Key.ParentDomain
                };

            var domainPatternStats =
                from e in _pingInputStream
                group e by new { e.SiteId } into domainGroups
                from win in domainGroups.HoppingWindow(
                    TimeSpan.FromSeconds(timeout),
                    TimeSpan.FromSeconds(timeout / 5),
                    HoppingWindowOutputPolicy.ClipToWindowEnd)
                select new DomainStatistics
                {
                    Id = domainGroups.Key.SiteId,
                    ParentId = null,
                    SelectorType = (int)PageSelectorType.DomainPattern,
                    Pattern = null,
                    Count = (int)win.Count()
                };

            var domainStatsTotal =
                from e in domainPatternStats
                select new DomainStatistics
                {
                    ParentId = e.Id,
                    Id = DomainSelector.GetDomainId(DomainSelector.TotalDomain, e.Id),
                    //Domain = string.Empty,
                    SelectorType = (int)PageSelectorType.DomainString,
                    //UserId = Guid.Empty,
                    Pattern = DomainSelector.TotalDomain,
                    Count = e.Count
                    //PageId = null
                };

            var pageStats =
                from e in _pingInputStream
                where e.PageId != null
                group e by new { e.PageId, e.ParentDomain, e.SiteId } into domainGroups
                from win in domainGroups.HoppingWindow(
                    TimeSpan.FromSeconds(timeout),
                    TimeSpan.FromSeconds(timeout / 5),
                    HoppingWindowOutputPolicy.ClipToWindowEnd)
                select new DomainStatistics
                {
                    ParentId = DomainSelector.GetDomainId(domainGroups.Key.ParentDomain, domainGroups.Key.SiteId),
                    Id = PageSelector.GetPageId(domainGroups.Key.PageId.Value, domainGroups.Key.ParentDomain, domainGroups.Key.SiteId),
                    Pattern = null,
                    SelectorType = (int)PageSelectorType.PagePattern,
                    Count = (int)win.Count()
                };

            var totalPageStats =
                from e in _pingInputStream
                where e.PageId != null
                group e by new { e.PageId, e.SiteId } into domainGroups
                from win in domainGroups.HoppingWindow(
                    TimeSpan.FromSeconds(timeout),
                    TimeSpan.FromSeconds(timeout / 5),
                    HoppingWindowOutputPolicy.ClipToWindowEnd)
                select new DomainStatistics
                {
                    ParentId = DomainSelector.GetDomainId(DomainSelector.TotalDomain, domainGroups.Key.SiteId),
                    Id = PageSelector.GetPageId(domainGroups.Key.PageId.Value, DomainSelector.TotalDomain, domainGroups.Key.SiteId),
                    Pattern = null,
                    SelectorType = (int)PageSelectorType.PagePattern,
                    Count = (int)win.Count()
                };

            var totalStats = domainPatternStats.Union(domainStats).Union(domainStatsTotal).Union(totalPageStats).Union(pageStats);

            _domainStatSource = totalStats.ToObservable();

            _statsOutputService = new CepObserver<DomainStatistics>();
            _eventsOutputService = new CepDataCollector<PageRequest>();

            _subscription = _domainStatSource.Subscribe(_statsOutputService);
        }

        public void Dispose()
        {
            if (_subscription != null) _subscription.Dispose();
            if (_subscription2 != null) _subscription2.Dispose();
            _cepApplication.Delete();
            _cepServer.Dispose();

            _disposed = true;
        }
    }
}
