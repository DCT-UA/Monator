using System;
using System.Collections.Generic;
using System.Linq;

namespace DCT.Monitor.Entities
{
    public class RequestSessionGroup: PageRequest
    {
        public List<PageRequest> AditionalRequests { get; set; }
        public int PageCount { get { return AditionalRequests.Count + 1; } }

        public RequestSessionGroup(PageRequest element, List<PageRequest> other)
        {
            Domain = element.Domain;
            Url = element.Url;
            IpAddress = element.IpAddress;
            Browser = element.Browser;
            Duration = element.Duration;
            Id = element.Id;
            SessionIdentifier = element.SessionIdentifier;
			SiteId = element.SiteId;
			UserId = element.UserId;

            AditionalRequests = other;
        }

        public static PageRequest GetEventFromGroup(IGrouping<Guid, PageRequest> group)
        {
            var element = group.First();
            var other = group.Skip(1).ToList();
            if (other.Count == 0) return element;
            else return new RequestSessionGroup(element, other);
        }
    }
}
