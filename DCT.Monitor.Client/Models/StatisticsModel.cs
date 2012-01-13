using System;
using System.Collections.Generic;
using System.Linq;
using DCT.ObjectModel;
using DCT.WPF.MVC;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Client.Models
{
    public class StatisticsModel: ViewModel
    {
        private IEnumerable<PageRequest> _requests;

        public IEnumerable<PageRequest> Requests
        {
            get { return _requests; }
            set { _requests = value; SendPropertyChanged("Requests"); }
        }

        private ObservableCollection<DomainStatisticsItemBase> _domains;
        private IEnumerable<DomainStatisticsItemBase> _oldData;

        public IEnumerable<DomainStatisticsItemBase> Domains
        {
            get { return _domains; }
            set
            {
                if (object.ReferenceEquals(_oldData, value)) return;
                _oldData = value;
                _domains.Merge(
                    _oldData,
                    comparer: (o1, o2) => o1.Id == o2.Id,
                    addAction: (n) => new DomainStatisticsItem(_domains.Dispatcher).CopyFrom(n),
                    updateAction: (n, o) => o.CopyFrom(n)
                );
                SendPropertyChanged("TotalCount");
            }
        }

        public int TotalCount { get { return Domains.Sum(i => i.Count); } }

        private DomainStatistics _selectedDomain;

        public DomainStatistics SelectedDomain
        {
            get { return _selectedDomain; }
            set
            {
                if (value != null)
                {
                    _selectedDomain = value;
                }
                SendSelectedDomainChanged();
            }
        }
        
        public StatisticsModel(ControllerContext context)
        {
            _domains = new ObservableCollection<DomainStatisticsItemBase>(context.ViewsDispatcher);
        }

        private void DomainsUpdated(object o, EventArgs e)
        {
            SendPropertyChanged("Domains");
            _domains.Dispatcher.BeginInvoke(new Action(SendSelectedDomainChanged));
        }

        private void SendSelectedDomainChanged()
        {
            SendPropertyChanged("SelectedDomain");
        }
    }
}
