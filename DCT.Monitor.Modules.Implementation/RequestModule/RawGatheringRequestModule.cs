using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using DCT.Unity;
using DCT.Utils;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;

namespace DCT.Monitor.Modules.Implementation.RequestModule
{
    public class RawGatheringRequestModule: IRequestModule
    {         
        private static int _roundIndex;
        private static GatheringRequestThread[] _handlers;
        private static GatheringRequestSlot[] _requestSlots;
        private static int _currentSlotIndex;
        private static IConfigurationModule _config;
        private static IDomainStatisticsModule _statisticsModule;
        private static DateTime _timeLine;
        private static Timer _statsTimer;
        private static int _failCount;
        private static GatheringRequestSlot _currentSlot;

        static RawGatheringRequestModule()
        {
            _config = ServiceLocator.Current.Resolve<IConfigurationModule>();
            _statisticsModule = ServiceLocator.Current.Resolve<IDomainStatisticsModule>();

            _handlers = Enumerable.Range(0, Environment.ProcessorCount * 2).Select(i => new GatheringRequestThread(i)).ToArray();

            _requestSlots = Enumerable.Range(1, 5).Select(i => new GatheringRequestSlot(_handlers.Length)).ToArray();
            _currentSlotIndex = 0;
            _currentSlot = _requestSlots[_currentSlotIndex];
            _timeLine = DateTime.Now.AddSeconds(_config.CoreDelay / 5.0);
            _statsTimer = new Timer(CountStats, null, 0, 100);
        }

        private static void CountStats(object o)
        {
            if (DateTime.Now >= _timeLine)
            {
                _timeLine = _timeLine.AddSeconds(_config.CoreDelay / 5.0);

                var slot = _requestSlots[_currentSlotIndex];
                if (_currentSlotIndex >= _requestSlots.Length - 1) _currentSlotIndex = 0;
                else _currentSlotIndex++;
                _currentSlot = _requestSlots[_currentSlotIndex];

                slot.Reset();
                var stats = new Dictionary<Guid, RawDomainStatistics>();
                _requestSlots.Each(i => i.Gather(stats));

                foreach (var statItem in stats.Values)
                {
                    var selector = (IPageSelector)statItem;
                    var statCount = statItem.Requests.ToList();
                    if (selector.SelectorType == (int)PageSelectorType.DomainPattern)
                    {
                        _statisticsModule.SetDomainRequests(new DomainSelector(DomainSelector.TotalDomain, selector.Id), statCount);
                    }
                    _statisticsModule.SetDomainRequests(selector, statCount);
                }
            }
        }

        internal static void ProcessRequest(int threadIndex, PageRequest request)
        {
            _currentSlot.ThreadSlots[threadIndex].ProcessRequest(request);
        }

        internal static bool Contains(IPageSelector selector)
        {
            return _statisticsModule.Contains(selector);
        }

        public void SendRequest(PageRequest request)
        {
            var i = _roundIndex;
            if (++i >= _handlers.Length << 4) i = 0;
            _roundIndex = i;

            //RequestHelper.ProcessRequest(request);
            _handlers[i >> 4].Enqueue(request);
        }
    }

    internal class GatheringCounter
    {
        public int Count;
    }

    internal class GatheringThreadSlot
    {
        private ConcurrentDictionary<Guid, RawDomainStatistics> _siteCounter;
        private ConcurrentDictionary<Guid, RawDomainStatistics> _domainCounter;
        private ConcurrentDictionary<Guid, RawDomainStatistics> _pageCounter;
        private ConcurrentDictionary<Guid, RawDomainStatistics> _pagePerDomainCounter;

        internal GatheringThreadSlot()
        {
            _siteCounter = new ConcurrentDictionary<Guid, RawDomainStatistics>();
            _domainCounter = new ConcurrentDictionary<Guid, RawDomainStatistics>();
            _pageCounter = new ConcurrentDictionary<Guid, RawDomainStatistics>();
            _pagePerDomainCounter = new ConcurrentDictionary<Guid, RawDomainStatistics>();
        }

        internal void ProcessRequest(PageRequest request)
        {
            var domainSelector = request.ParentDomain == null ? null : new DomainSelector(request.ParentDomain, request.SiteId);
            AddOrUpdateCounter(_siteCounter, new Site { Id = request.SiteId }, request);
            if(domainSelector != null) AddOrUpdateCounter(_domainCounter, domainSelector, request);

            if (request.PageId != null)
            {
                var page = new Page{Id = request.PageId.Value};
                var totalDomainSelector = new DomainSelector(DomainSelector.TotalDomain, request.SiteId);
                var totalPageSelector = new PageSelector(page, totalDomainSelector);

                AddOrUpdateCounter(_pageCounter, totalPageSelector, request);
                if (domainSelector != null)
                {
                    var pageSelector = new PageSelector(page, domainSelector);
                    AddOrUpdateCounter(_pagePerDomainCounter, pageSelector, request);
                }
            }
        }

        internal void Reset(Dictionary<Guid, RawDomainStatistics> stats)
        {
            Reset(_domainCounter, stats);
            Reset(_siteCounter, stats);
            Reset(_pageCounter, stats);
            Reset(_pagePerDomainCounter, stats);
        }

        private void Reset(ConcurrentDictionary<Guid, RawDomainStatistics> counters, Dictionary<Guid, RawDomainStatistics> stats)
        {
            foreach (var key in counters.Keys)
            {
                var value = counters[key];
                counters[key] = new RawDomainStatistics((IPageSelector)value);

                RawDomainStatistics stat;
                if (!stats.TryGetValue(key, out stat)) stats.Add(key, stat = new RawDomainStatistics((IPageSelector)value));

                stat.AddItemsInCounter(value.Requests);
            }
        }

        private void AddOrUpdateCounter(ConcurrentDictionary<Guid, RawDomainStatistics> dictionary, IPageSelector data, PageRequest request)
        {                              
            if(RawGatheringRequestModule.Contains(data))
            {
                RawDomainStatistics counter = dictionary.GetOrAdd(data.Id, new RawDomainStatistics(data));
                counter.AddItemInCounter(request);
            }
        }
    }

    internal class GatheringRequestSlot
    {
        public GatheringThreadSlot[] ThreadSlots { get; private set; }
        public Dictionary<Guid, RawDomainStatistics> Stats { get; set; }
        public DateTime Expired { get; set; }

        public GatheringRequestSlot(int threadsCount)
        {
            ThreadSlots = Enumerable.Range(0, threadsCount).Select(i => new GatheringThreadSlot()).ToArray();
            Stats = new Dictionary<Guid, RawDomainStatistics>();
        }

        public void Reset()
        {
            Stats = new Dictionary<Guid, RawDomainStatistics>();

            foreach (var threadSlot in ThreadSlots)
            {
                threadSlot.Reset(Stats);
            }

            /*return stats.ToDictionary(i => (IPageSelector)i.Value, i => new DomainStatisticsData(i.Value));*/
        }

        public void Gather(Dictionary<Guid, RawDomainStatistics> stats)
        {
            foreach (var key in Stats.Keys)
            {
                var value = Stats[key];

                RawDomainStatistics stat;
                if (!stats.TryGetValue(key, out stat)) stats.Add(key, stat = new RawDomainStatistics((IPageSelector)value));

                stat.AddItemsInCounter(value.Requests);
            }
        }

        private List<PageRequest> ResetRequestsDictionary(ConcurrentDictionary<Guid, PageRequest> values)
        {
            var data = values.Values.ToList();
            values.Clear();

            return data;
        }
    }

    internal class GatheringRequestThread
    {
        private Thread _workThread;
        private ConcurrentQueue<PageRequest> _requestQueue;
        private int _index;

        public GatheringRequestThread(int i)
        {
            _index = i;
            _requestQueue = new ConcurrentQueue<PageRequest>();
            _workThread = new Thread(Start);
            _workThread.Start();
        }

        public void Enqueue(PageRequest request)
        {
            _requestQueue.Enqueue(request);
        }

        private void Start()
        {
            while (_workThread.ThreadState == ThreadState.Running)
            {
                PageRequest request;
                if (_requestQueue.TryDequeue(out request) && request != null)
                {
                    RawGatheringRequestModule.ProcessRequest(_index, request);
                }
                else Thread.Sleep(100);
            }
        }
    }

    static class GatheringCircularLinkedList
    {
        public static bool TryDequeue<T>(this Queue<T> t, out T request)
        {
            request = default(T);
            if (t.Count == 0) return false;
            request = t.Dequeue();

            return true;
        }

        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            if (current.Next == null)
                return current.List.First;
            return current.Next;
        }

        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
        {
            if (current.Previous == null)
                return current.List.Last;
            return current.Previous;
        }
    }
}
