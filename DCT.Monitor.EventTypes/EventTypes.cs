using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using DCT.ObjectModel;

namespace Inetgiant.Monitor.Entities
{
    [Serializable]
    public class PageRequest: IEntity<Guid>
    {
        public string Domain { get; set; }
        public string Url { get; set; }
        public string IpAddress { get; set; }
        public string Refferer { get; set; }
        public byte Browser { get; set; }
        public Guid Id { get; set; }
        public Guid SessionIdentifier { get; set; }
        public TimeSpan Duration { get; set; }
		public string DurationString 
		{
			get { return Duration.ToString();}
			set 
			{ 
				Duration = TimeSpan.Parse(value); 
			} 
		}
        public Guid SiteId { get; set; }
		public Guid UserId { get; set; }
        public Guid? PageId { get; set; }

        public override bool Equals(object obj)
        {
            var ping2 = obj as PageRequest;
            return (ping2 == null) ? false : ping2.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

	public class PageRequestEventArgs : EventArgs
	{
		public PageRequest Click { get; set; }
	}

	[Serializable]
    public class DomainStatistics: NotifyObject
    {
        public DomainStatistics()
        {
        }

        public DomainStatistics(IPageSelector selector)
        {
            if (selector == null) throw new NullReferenceException("Selector is null");

            var site = selector as Site;
            if (site != null)
            {
                SelectorType = (int)PageSelectorType.DomainPattern;
                Domain = site.Domain;
                SiteId = site.Id;
                UserId = site.UserId;
                return;
            }

            var page = selector as Page;
            if (page != null)
            {
                SelectorType = (int)PageSelectorType.PagePattern;
                Domain = page.PagePattern;
                SiteId = page.DomainId;
                PageId = page.Id;
                return;
            }

            var domainSelector = selector as DomainSelector;
            if (domainSelector != null)
            {
                SelectorType = (int)PageSelectorType.DomainString;
                Domain = domainSelector.Pattern;
                SiteId = domainSelector.ParentId.Value;
                return;
            }

            throw new InvalidOperationException("Unsupported selector type specified " + selector.GetType().Name);
        }

		public Guid SiteId { get; set; }
		public Guid UserId { get; set; }
		public string Domain { get; set; }
        public int Count { get; set; }
        public int SelectorType { get; set; }
        public Guid? PageId { get; set; }
    }

    public enum PageSelectorType
    {
        DomainPattern,
        DomainString,
        PagePattern
    }

	[Serializable]
	public class LocationResult
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public int Count { get; set; }
	}

	public static class PageRequestHelper
	{
		public static Site GetSite(this PageRequest pageRequest)
		{
			return new Site() { Id = pageRequest.SiteId, UserId = pageRequest.UserId, Domain = pageRequest.Domain };
		}
	}

	public static class DomainStatisticsHelper
	{
		public static IPageSelector GetSelector(this DomainStatistics domainStat)
		{
            switch ((PageSelectorType)domainStat.SelectorType)
            {
                case PageSelectorType.DomainPattern:
                    return new Site() { Id = domainStat.SiteId, UserId = domainStat.UserId, Domain = domainStat.Domain };

                case PageSelectorType.DomainString:
                    return new DomainSelector(domainStat.Domain, domainStat.SiteId);

                case PageSelectorType.PagePattern:
                    return new Page() { Id = domainStat.PageId.Value };
            }

            throw new InvalidOperationException();
		}
	}
}
