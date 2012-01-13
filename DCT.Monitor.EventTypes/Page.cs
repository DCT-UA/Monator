using System;
using System.Text.RegularExpressions;

namespace DCT.Monitor.Entities
{
    [Serializable]
    public class Page : IEntity<Guid>
    {
        private Regex _regexpPattern;

        public Guid Id { get; set; }
        public Guid DomainId { get; set; }
        public string PagePattern { get; set; }

        public bool Check(string urlPath)
        {
            _regexpPattern = _regexpPattern ?? new Regex(@"\/?" + Regex.Escape(PagePattern).Replace("*", ".*") + @"(\?.*)?");

            return _regexpPattern.IsMatch(urlPath);
        }
    }

    [Serializable]
    public class PageSelector : IPageSelector
    {
        public PageSelector(Page page, string domain)
            :this(page, new DomainSelector(domain, page.DomainId))
        {
        }

        public PageSelector(Page page, DomainSelector parentDomain)
        {
            Id = GetPageId(page.Id, parentDomain.Id);
            ParentId = parentDomain.Id;

            SiteId = page.DomainId;
            PageId = page.Id;
            Pattern = page.PagePattern;
            ParentDomain = parentDomain;
        }

        /// <summary>
        /// Gets Selector Id for page entity
        /// </summary>
        /// <param name="pageId">Id of page entity</param>
        /// <param name="parentId">Id of parent selector</param>
        /// <returns>Id of page selector</returns>
        public static Guid GetPageId(Guid pageId, Guid parentId)
        {
            var bytes = pageId.ToByteArray();
            var parentBytes = parentId.ToByteArray();
            for (var i = 0; i < bytes.Length; i++) bytes[i] ^= parentBytes[i];

            return new Guid(bytes);
        }

        /// <summary>
        /// Gets Selector Id for page entity
        /// </summary>
        /// <param name="pageId">Id of page entity</param>
        /// <param name="domain">domain of request</param>
        /// <param name="siteId">Id of parent site entity</param>
        /// <returns>Id of page selector</returns>
        public static Guid GetPageId(Guid pageId, string domain, Guid siteId)
        {
            return GetPageId(pageId, DomainSelector.GetDomainId(domain, siteId));
        }

        public int SelectorType
        {
            get { return (int)PageSelectorType.PagePattern; }
            set { throw new NotSupportedException(); }
        }

        public Guid Id { get; set; }
        public string Pattern { get; set; }
        public Guid? ParentId { get; set; }
        public Guid SiteId { get; set; }
        public Guid PageId { get; set; }
        public DomainSelector ParentDomain { get; set; }
    }
}
