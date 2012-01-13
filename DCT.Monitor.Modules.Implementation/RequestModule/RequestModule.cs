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
    public class RequestModule: BaseRequestModule
    {
        private static int _roundIndex;
        private static RequestThread[] _handlers;
        private static RequestSlot[] _requestSlots;
        private static int _currentSlotIndex;
        private static IConfigurationModule _config;
        private static IDomainStatisticsModule _statisticsModule;
        private static DateTime _timeLine;
        private static Timer _statsTimer;
        private static int _failCount;
        private static RequestSlot _currentSlot;

        static RequestModule()
        {
            _config = ServiceLocator.Current.Resolve<IConfigurationModule>();
            _statisticsModule = ServiceLocator.Current.Resolve<IDomainStatisticsModule>();

            _handlers = Enumerable.Range(0, Environment.ProcessorCount * 2).Select(i => new RequestThread(i)).ToArray();

            _requestSlots = Enumerable.Range(1, 5).Select(i => new RequestSlot(_handlers.Length)).ToArray();
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

                if (DateTime.Now >= _timeLine)
                {
                    if (++_failCount > 5)
                    {
                        _config.CoreDelay *= 2;
                        _failCount = 0;
                    }
                }
                else _failCount = 0;

                var slot = _requestSlots[_currentSlotIndex];
                if (_currentSlotIndex >= _requestSlots.Length - 1) _currentSlotIndex = 0;
                else _currentSlotIndex++;
                _currentSlot = _requestSlots[_currentSlotIndex];

                slot.Reset();
                var stats = new Dictionary<Guid, DomainStatistics>();
                _requestSlots.Each(i => i.Count(stats));

                foreach (var statItem in stats.Values)
                {
                    var selector = (IPageSelector)statItem;
                    var statCount = new DomainStatisticsData(statItem);
                    if (selector.SelectorType == (int)PageSelectorType.DomainPattern)
                    {
                        _statisticsModule.SetDomainStatistics(new DomainSelector(DomainSelector.TotalDomain, selector.Id), statCount);
                    }
                    _statisticsModule.SetDomainStatistics(selector, statCount);
                }
            }
        }

        internal static void ProcessRequest(int threadIndex, PageRequest request)
        {
            _currentSlot.ThreadSlots[threadIndex].ProcessRequest(request);
        }
    
        protected override void OnSendRequest(PageRequest request)
        {
            
            var i = _roundIndex;
            if(++i >= _handlers.Length << 4) i = 0;
            _roundIndex = i;

            RequestHelper.ProcessRequest(request);
            _handlers[i >> 4].Enqueue(request);
        }
    }

    internal class Counter
    {
        public int Count;
    }

    internal class ThreadSlot{
        private ConcurrentDictionary<Guid, DomainStatistics> _siteCounter;
        private ConcurrentDictionary<Guid, DomainStatistics> _domainCounter;
        private ConcurrentDictionary<Guid, DomainStatistics> _pageCounter;
        private ConcurrentDictionary<Guid, DomainStatistics> _pagePerDomainCounter;

        internal ThreadSlot()
        {
            _siteCounter = new ConcurrentDictionary<Guid, DomainStatistics>();
            _domainCounter = new ConcurrentDictionary<Guid, DomainStatistics>();
            _pageCounter = new ConcurrentDictionary<Guid, DomainStatistics>();
            _pagePerDomainCounter = new ConcurrentDictionary<Guid, DomainStatistics>();
        }

        internal void ProcessRequest(PageRequest request)
        {
            var domainSelector = request.ParentDomain == null ? null : new DomainSelector(request.ParentDomain, request.SiteId);
            AddOrUpdateCounter(_siteCounter, new Site { Id = request.SiteId });
            if(domainSelector != null) AddOrUpdateCounter(_domainCounter, domainSelector);

            if (request.PageId != null)
            {
                var page = new Page{Id = request.PageId.Value};
                var totalDomainSelector = new DomainSelector(DomainSelector.TotalDomain, request.SiteId);
                var totalPageSelector = new PageSelector(page, totalDomainSelector);

                AddOrUpdateCounter(_pageCounter, totalPageSelector);
                if (domainSelector != null)
                {
                    var pageSelector = new PageSelector(page, domainSelector);
                    AddOrUpdateCounter(_pagePerDomainCounter, pageSelector);
                }
            }
        }

        internal void Reset(Dictionary<Guid, DomainStatistics> stats)
        {
            Reset(_domainCounter, stats);
            Reset(_siteCounter, stats);
            Reset(_pageCounter, stats);
            Reset(_pagePerDomainCounter, stats);
        }

        private void Reset(ConcurrentDictionary<Guid, DomainStatistics> counters, Dictionary<Guid, DomainStatistics> stats)
        {
            foreach (var key in counters.Keys)
            {
                var value = counters[key];
                counters[key] = new DomainStatistics((IPageSelector)value);

                DomainStatistics stat;
                if (!stats.TryGetValue(key, out stat)) stats.Add(key, stat = new DomainStatistics((IPageSelector)value));

                stat.Count += value.Count;
            }
        }

        private void AddOrUpdateCounter(ConcurrentDictionary<Guid, DomainStatistics> dictionary, IPageSelector data)
        {
            DomainStatistics counter = dictionary.GetOrAdd(data.Id, new DomainStatistics(data));
            counter.Increment();
        }
    }

    internal class RequestSlot
    {
        public ThreadSlot[] ThreadSlots { get; private set; }
        public Dictionary<Guid, DomainStatistics> Stats { get; set; }
        public DateTime Expired { get; set; }

        public RequestSlot(int threadsCount)
        {
            ThreadSlots = Enumerable.Range(0, threadsCount).Select(i => new ThreadSlot()).ToArray();
            Stats = new Dictionary<Guid, DomainStatistics>();
        }

        public void Reset()
        {
            Stats = new Dictionary<Guid, DomainStatistics>();

            foreach (var threadSlot in ThreadSlots)
            {
                threadSlot.Reset(Stats);
            }

            /*return stats.ToDictionary(i => (IPageSelector)i.Value, i => new DomainStatisticsData(i.Value));*/
        }

        public void Count(Dictionary<Guid, DomainStatistics> stats)
        {
            foreach (var key in Stats.Keys)
            {
                var value = Stats[key];

                DomainStatistics stat;
                if (!stats.TryGetValue(key, out stat)) stats.Add(key, stat = new DomainStatistics((IPageSelector)value));

                stat.Count += value.Count;
            }
        }

        private List<PageRequest> ResetRequestsDictionary(ConcurrentDictionary<Guid, PageRequest> values)
        {
            var data = values.Values.ToList();
            values.Clear();

            return data;
        }
    }

    internal class RequestThread
    {
        private Thread _workThread;
        private ConcurrentQueue<PageRequest> _requestQueue;
        private int _index;

        public RequestThread(int i)
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
                    RequestModule.ProcessRequest(_index, request);
                }
                else Thread.Sleep(100);
            }
        }
    }

    static class CircularLinkedList
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
