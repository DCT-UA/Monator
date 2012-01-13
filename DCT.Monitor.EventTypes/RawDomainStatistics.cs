using System.Collections.Concurrent;
using DCT.Utils;

namespace DCT.Monitor.Entities
{
    public class RawDomainStatistics: DomainStatistics
    {
        private ConcurrentQueue<PageRequest> _requests = new ConcurrentQueue<PageRequest>(); 
        public RawDomainStatistics() 
        {
        }

        public RawDomainStatistics(IPageSelector selector):base(selector) 
        {
        }

        public RawDomainStatistics(DomainStatistics stats):base(stats) 
        {
        }
        
        
        public ConcurrentQueue<PageRequest> Requests
        {
            get { return _requests; }
            set { _requests = value; }
        }

        public void AddItemInCounter(PageRequest request)
        {
            Requests.Enqueue(request);
        }

        public void AddItemsInCounter(ConcurrentQueue<PageRequest> concurrentQueue)
        {
            concurrentQueue.Each(Requests.Enqueue);
        }
    }
}
