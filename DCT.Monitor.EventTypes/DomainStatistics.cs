using System;
using DCT.ObjectModel;
using System.Threading;

namespace DCT.Monitor.Entities
{
    [Serializable]
    public class DomainStatisticsData
    {
        public DomainStatisticsData()
        {
        }

        public DomainStatisticsData(DomainStatistics item)
        {
            Count = item.Count;
        }

        public int Count { get; set; }
    }

    [Serializable]
    public class DomainStatistics : NotifyObject, ICopyable<DomainStatistics>, IPageSelector
    {
        public DomainStatistics()
        {
        }

        public DomainStatistics(IPageSelector selector)
        {
            selector.CopyTo(this);
        }

        public DomainStatistics(DomainStatistics stats)
        {
            stats.CopyTo(this);
        }

        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                SendPropertyChanged(() => this.Id);
            }
        }

        private Guid? _parentId;
        public Guid? ParentId
        {
            get { return _parentId; }
            set
            {
                if (_parentId == value) return;
                _parentId = value;
                SendPropertyChanged(() => this.ParentId);
            }
        }
    
        private string _pattern;
        public string Pattern
        {
            get { return _pattern; }
            set
            {
                if (_pattern == value) return;
                _pattern = value;
                SendPropertyChanged(() => this.Pattern);
            }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
            set
            {
                if (_count == value) return;
                _count = value;
                SendPropertyChanged(() => this.Count);
            }
        }

        
        

        public int SelectorType { get; set; }

        public void Increment()
        {
            _count++;
        }

        


        public void SyncIncrement()
        {
            Interlocked.Increment(ref _count);
        }


        public static void Copy(DomainStatistics source, DomainStatistics target)
        {
            IPageSelectorHelper.Copy(source, target);
            target.Count = source.Count;
        }

        
    }
}
