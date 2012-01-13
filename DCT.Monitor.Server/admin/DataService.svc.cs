using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Inetgiant.Monitor.Entities;
using Inetgiant.Monitor.Modules;
using My.Unity;
using System.Web.Security;

namespace Skyner.Server.admin
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DataService" in code, svc and config file together.
    public class DataService : IDataService
    {
        public static IDomainStatisticsDataSourceModule _dataSource = ServiceLocator.Current.Resolve<IDomainStatisticsDataSourceModule>();
        public static IUserModule _userModule = ServiceLocator.Current.Resolve<IUserModule>();
        public static IGeolocationModule _geolocation = ServiceLocator.Current.Resolve<IGeolocationModule>();

        public DomainStatistics[] GetDomainStatistics()
        {
            return ToArray(_dataSource.GetDomainStatistics());
        }

		public DomainStatistics[] GetDomainStatisticsByUser(User user)
		{
            if (_userModule.GetUser(user.UserName).IsAdministrator)
                return ToArray(_dataSource.GetDomainStatistics());
            else
    			return ToArray(_dataSource.GetDomainStatistics().Where(i => i.UserId == user.Id));
		}

        public PageRequest[] GetDomainRequests(Site site)
        {
            return ToArray(_dataSource.GetDomainRequests(site));
        }

		public List<LocationResult> GetLocations()
		{
			var statistics = GetDomainStatistics();
			List<PageRequest> result = new List<PageRequest>();
			foreach (var statistic in statistics)
			{
                var data = GetDomainRequests(statistic.GetSite());
				if(data != null) result.AddRange(data);
			}

            return _geolocation.Convert(result);
		}

        public User LoginUser(User user)
        {
            if (!_userModule.Authenticate(user)) return null;
            return user;
        }

        private T[] ToArray<T>(IEnumerable<T> Source)
        {
            return (Source == null) ? null : Source.ToArray();
        }
    }
}
