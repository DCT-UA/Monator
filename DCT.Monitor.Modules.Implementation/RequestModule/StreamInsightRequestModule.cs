using System;
using DCT.Unity;
using DCT.Monitor.Adapters;
using DCT.Monitor.Entities;
using DCT.Monitor.StreamInsight;
using DCT.Monitor.Modules;
using Monitor.DAL;

namespace DCT.Monitor.Modules.Implementation.RequestModule
{
    public class StreamInsightRequestModule : BaseRequestModule
    {
        private static QueriesCore _streamInsightCore;
        private static IDomainStatisticsModule _domainStatistics;
        private static IRequestRepository _requestsRepository;
        private static IConfigurationModule _config;

        static StreamInsightRequestModule()
        {
            _streamInsightCore = ServiceLocator.Current.Resolve<QueriesCore>();
            _domainStatistics = ServiceLocator.Current.Resolve<IDomainStatisticsModule>();
            _requestsRepository = ServiceLocator.Current.Resolve<IRequestRepository>();
            _config = ServiceLocator.Current.Resolve<IConfigurationModule>();

            _streamInsightCore.RegisterCounting();

            _streamInsightCore.DomainStatisticsOutputService.Next += DomainStatisticsHandler;
        }

        private static void DomainStatisticsHandler(object sender, EventArgs<DomainStatistics> e)
        {
            try
            {
                var data = e.Data;
                //ServiceLocator.LoggerService.Info(string.Format("Id:{0}, Count:{1}, Date: {2}",data.Id,data.Count, DateTime.Now.TimeOfDay));
                _domainStatistics.SetDomainStatistics(e.Data, new DomainStatisticsData(e.Data));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        protected override void OnSendRequest(PageRequest request)
        {
            RequestHelper.ProcessRequest(request);

            //if (request.Id == default(Guid)) _requestsRepository.Create(request);
            _streamInsightCore.PingInputService.PushEvent(request);
        }
    }
}
