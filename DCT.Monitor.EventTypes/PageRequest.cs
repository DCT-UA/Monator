using System;
using DCT.ObjectModel;

namespace DCT.Monitor.Entities
{
    [Serializable]
    public class PageRequest : NotifyObject, IEntity<Guid>
    {
        private string _domain;
        public string Domain { get { return _domain; } set { _domain = value; SendPropertyChanged(() => this.Domain); } }

        private string _url;
        public string Url { get { return _url; } set { _url = value; SendPropertyChanged(() => this.Url); } }

        private string _ipAddress;
        public string IpAddress { get { return _ipAddress; } set { _ipAddress = value; SendPropertyChanged(() => this.IpAddress); } }

        private string _refferer;
        public string Refferer { get { return _refferer; } set { _refferer = value; SendPropertyChanged(() => this.Refferer); } }

        private byte _browser;
        public byte Browser { get { return _browser; } set { _browser = value; SendPropertyChanged(() => this.Browser); } }

        private Guid _id;
        public Guid Id { get { return _id; } set { _id = value; SendPropertyChanged(() => this.Id); } }

        private Guid _sessionIdentifier { get; set; }
        public Guid SessionIdentifier { get { return _sessionIdentifier; } set { _sessionIdentifier = value; SendPropertyChanged(() => this.SessionIdentifier); } }

        private TimeSpan _duration { get; set; }
        public TimeSpan Duration { get { return _duration; } set { _duration = value; SendPropertyChanged(() => this.Duration); SendPropertyChanged(() => this.DurationString); } }

        public string DurationString
        {
            get { return Duration.ToString(); }
            set
            {
                Duration = TimeSpan.Parse(value);
            }
        }
        private Guid _siteId { get; set; }
        public Guid SiteId { get { return _siteId; } set { _siteId = value; SendPropertyChanged(() => this.SiteId); } }

        private Guid _userId { get; set; }
        public Guid UserId { get { return _userId; } set { _userId = value; SendPropertyChanged(() => this.UserId); } }

        private Guid? _pageId { get; set; }
        public Guid? PageId { get { return _pageId; } set { _pageId = value; SendPropertyChanged(() => this.PageId); } }

        public override bool Equals(object obj)
        {
            var ping2 = obj as PageRequest;
            return (ping2 == null) ? false : ping2.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public string ParentDomain { get; set; }
    }

    public class PageRequestEventArgs : EventArgs
    {
        public PageRequest Click { get; set; }
    }
    public enum PageSelectorType
    {
        DomainPattern,
        DomainString,
        PagePattern
    }
    public static class PageRequestHelper
    {
        public static Site GetSite(this PageRequest pageRequest)
        {
            return new Site() { Id = pageRequest.SiteId, UserId = pageRequest.UserId, Domain = pageRequest.Domain };
        }
    }
}
