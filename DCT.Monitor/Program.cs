using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using DCT.Unity;
using System.Threading;
using DCT.Monitor.Adapters;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using DCT.Monitor.Modules.Implementation.RequestModule;
using Microsoft.Practices.Unity;

namespace DCT.Monitor
{
	class Test
	{
        static IRequestModule core;
        static string[] domains = new[] { "inetgiant.net", "inetgiant.com", "inetgiant.co.uk", "inetgiant.com.co", "inetgiant.pk", "inetgiant.ru" };
        static Guid siteGuid;
        static Guid?[] pageGuids = Enumerable.Range(1, 10).Select(i => (Guid?)Guid.NewGuid()).ToArray();
        static Stopwatch sw1;
        static Stopwatch sw2;

        static Test()
        {
            var config = ServiceLocator.Current.Resolve<IConfigurationModule>();
            config.CoreDelay = 5;
            ServiceLocator.Current.Configure(Configurator);
        }

        public static void Configurator(UnityContainer c)
        {
            c.RegisterType(typeof(IDomainStatisticsModule), typeof(PrintCache));
        }

        private static Timer _timer;

        static void Main(string[] args)
        {
            //core = new StreamInsightRequestModule();
            core = new RequestModule();
            var siteRep = ServiceLocator.Current.Resolve<ISiteManagerModule>();
            var site = siteRep.GetSite("*inetgiant*");
            var user = ServiceLocator.Current.Resolve<IUserModule>().GetUser("drdoom");
            if (site == null)
            {
                site = new Site { Domain = "*inetgiant*", UserId = user.Id };
                siteRep.CreateSite(user, site);
            }
            siteGuid = site.Id;

            _timer = new Timer(CoreTest, null, 0, 1000);

            while (!Console.KeyAvailable)
            {
                Thread.Sleep(1000);
            }
        }

        static void CoreTest(object e)
        {
            try
            {
                sw2 = Stopwatch.StartNew();
                Enumerable.Range(1, 100).AsParallel().AsUnordered().ForAll(SendRequest);
                sw2.Stop();
                Console.WriteLine("\n\ninput: {0}", sw2.Elapsed);
            }
            catch(Exception ex)
            {
                ServiceLocator.LoggerService.Error(ex);
            }
        }

        private static void OutputStats(object sender, EventArgs<DomainStatistics> args)
        {
            if (sw1.IsRunning)
            {
                sw1.Stop();
                Console.WriteLine("output: {0}", sw1.Elapsed);
            }
        }

        private static void SendRequest(int index)
        {
            var rand = new Random(Environment.TickCount + index);

            var r = new PageRequest
            {
                Id = Guid.NewGuid(),
                Domain = domains[rand.Next(0, domains.Length - 1)],
                SiteId = siteGuid,
                PageId = rand.Next(0, 1) == 0 ? null : pageGuids[rand.Next(0, pageGuids.Length - 1)],
                Refferer = "",
                Browser = 1,
                Duration = TimeSpan.FromSeconds(5),
                IpAddress = "12312",
                Url = "http://asfasdf.asd", SessionIdentifier=Guid.Empty,
                UserId = Guid.Empty
            };
            r.ParentDomain = r.Domain;

            core.SendRequest(r);
        }
	}

    public class PrintCache : IDomainStatisticsModule
    {
        public void SetDomainStatistics(IPageSelector selector, DomainStatisticsData domainStatistics)
        {
            Console.Write("{0}: {1}\t", selector.Id, domainStatistics.Count);
        }

        public void SetDomainRequests(IPageSelector selector, IEnumerable<PageRequest> requests)
        {
        }

        public DomainStatisticsData GetDomainStatistics(IPageSelector selector)
        {
            return null;
        }

        public IEnumerable<PageRequest> GetDomainRequests(IPageSelector selector)
        {
            return null;
        }


        public bool Contains(IPageSelector selector)
        {
            throw new NotImplementedException();
        }
    }
}
