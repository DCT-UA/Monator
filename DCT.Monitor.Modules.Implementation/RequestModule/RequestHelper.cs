using System.Linq;
using DCT.Monitor.Entities;
using Monitor.DAL;
using DCT.Unity;
using System.Text.RegularExpressions;

namespace DCT.Monitor.Modules.Implementation.RequestModule
{
    public class RequestHelper
    {
        private static ISiteRepository _sitesRepository;
        private static IPageRepository _pagesRepository;

        static RequestHelper()
        {
            _sitesRepository = ServiceLocator.Current.Resolve<ISiteRepository>();
            _pagesRepository = ServiceLocator.Current.Resolve<IPageRepository>();
        }

        public static void ProcessRequest(PageRequest request)
        {
            if (request.Domain == null) return;
            var site = _sitesRepository.GetById(request.SiteId);
            var pages = _pagesRepository.GetPagesBySiteId(request.SiteId);
            //if (!request.Domain.EndsWith(site.Domain, StringComparison.InvariantCultureIgnoreCase)) return;

            var regex = "[a-z_.-]*";
            var pattern = site.Domain.Replace("*", regex);

            if (site.ContainsSubdomains)
            {
                pattern = @"([a-z_-]+\.)?" + pattern;
            }
            else
            {
                pattern = @"(www\.)?" + pattern;
            }

            var match = Regex.Match(request.Domain, pattern);
            if (match == null || !match.Success) return;

            request.ParentDomain = request.Domain;

            if (match.Groups.Count == 2 && match.Groups[1].Success)
            {
                request.ParentDomain = request.ParentDomain.Substring(match.Groups[1].Length);
            }

            request.ParentDomain = request.ParentDomain ?? "dummy";

            var page = pages.FirstOrDefault(i => i.Check(request.Url));
            if (page != null) request.PageId = page.Id;
        }
    }
}
