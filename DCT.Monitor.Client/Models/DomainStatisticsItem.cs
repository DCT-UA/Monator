using System.Collections.Generic;
using DCT.ObjectModel;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Client.Models
{
    public abstract class DomainStatisticsItemBase : DomainStatistics, ICopyable<DomainStatisticsItemBase>
    {
        private int _maxCount;

        public abstract IEnumerable<DomainStatisticsItemBase> ChildItems { get; set; }

        public int MaxCount
        {
            get { return _maxCount; }
            set { _maxCount = value; SendPropertyChanged(() => this.MaxCount); }
        }

        public IPageSelector Selector { get { return this; } }

        public static void Copy(DomainStatisticsItemBase source, DomainStatisticsItemBase target)
        {
            DomainStatistics.Copy(source, target);
            target.ChildItems = source.ChildItems;
            target.MaxCount = source.MaxCount;
        }
    }

    public class DomainStatisticsItem: DomainStatisticsItemBase
    {
        private ObservableCollection<DomainStatisticsItemBase> _childItems;
        private System.Windows.Threading.Dispatcher dispatcher;

        public override IEnumerable<DomainStatisticsItemBase> ChildItems
        {
            get { return _childItems; }
            set { 
                _childItems.Merge(
                    value,
                    comparer: (o1, o2) => o1.Id == o2.Id,
                    addAction: (n) => new DomainStatisticsItem().CopyFrom(n),
                    updateAction: (n, o) => o.CopyFrom(n)
                );
            }
        }

        public DomainStatisticsItem()
        {
            _childItems = new ObservableCollection<DomainStatisticsItemBase>();
        }

        public DomainStatisticsItem(System.Windows.Threading.Dispatcher collectionDispatcher)
        {
            _childItems = new ObservableCollection<DomainStatisticsItemBase>(collectionDispatcher);
        }
    }

    public class DomainStatisticsDataItem : DomainStatisticsItemBase
    {
        private IEnumerable<DomainStatisticsItemBase> _childItems;

        public override IEnumerable<DomainStatisticsItemBase> ChildItems
        {
            get { return _childItems; }
            set { _childItems = value; SendPropertyChanged(() => this.ChildItems); }
        }
    }
}
