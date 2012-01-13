using System;
using System.Text;
using DCT.Utils;
using DCT.ObjectModel;
using DCT.Unity;

namespace DCT.Monitor.Entities
{
    [Copy(typeof(IPageSelectorHelper), "Copy")]
    public interface IPageSelector: ICopyable<IPageSelector>
    {
        Guid Id { get; set; }
        Guid? ParentId { get; set; }
        string Pattern { get; set; }
        int SelectorType { get; set; }
    }

    public static class IPageSelectorHelper
    {
        public static void Copy(IPageSelector source, IPageSelector target)
        {
            target.Id = source.Id;
            target.Pattern = source.Pattern;
            target.ParentId = source.ParentId;
            target.SelectorType = source.SelectorType;
        }
    }

    public class DomainSelector : IPageSelector
    {
        public Guid Id { get; set; }
        public string Pattern { get; set; }
        public Guid? ParentId { get; set; }

        public DomainSelector(string domain, Guid parentId)
        {
            ParentId = parentId;
            Pattern = domain;
            Id = GetDomainId(domain, parentId);
        }

        /// <summary>
        /// Fake domain name wich used to group all domains within site
        /// </summary>
        public const string TotalDomain = "total";

        public static Guid GetDomainId(string domain, Guid siteId)
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                ServiceLocator.LoggerService.Info("Invalid domain: '" + domain + "'");
                domain = "undefined";
            }

            var hash = new StringBuilder(domain.HashString());

            hash.Insert(8, '-');
            hash.Insert(13, '-');
            hash.Insert(18, '-');
            hash.Insert(23, '-');

            var id = Guid.Parse(hash.ToString());

            // merge id and parrent id with XOR to make same domain have different ID for different parrents
            var bytes = id.ToByteArray();
            var parentBytes = siteId.ToByteArray();
            for (var i = 0; i < bytes.Length; i++) bytes[i] ^= parentBytes[i];

            return new Guid(bytes);
        }

        public DomainSelector(Guid id)
        {
            Pattern = "";
            Id = id;
        }

        public int SelectorType
        {
            get { return (int)PageSelectorType.DomainString; }
            set { throw new NotSupportedException(); }
        }
    }
}
