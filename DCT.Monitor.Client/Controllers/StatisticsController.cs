using DCT.Monitor.Client.Models;
using DCT.WPF.MVC;

namespace DCT.Monitor.Client.Controllers
{
    public class StatisticsController: Controller
    {
        private static ServiceProxy _dataSource;

        static StatisticsController()
        {
            _dataSource = new ServiceProxy();
        }

        [Async(IsBackground = true)]
        public StatisticsModel GetStatistics(StatisticsModel model)
        {
            if (model == null) model = new StatisticsModel(Context);

            
            model.Domains = _dataSource.GetDomainStats();
            if(model.SelectedDomain != null) model.Requests = _dataSource.GetRequests(model.SelectedDomain);

            return model;
        }

        public void SelectDomain(object domain)
        {
            var model = Context.Model as StatisticsModel;
            model.SelectedDomain = domain as DomainStatisticsItem;
        }
    }
}
