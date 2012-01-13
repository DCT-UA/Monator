using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Inetgiant.Monitor.Entities;

namespace Skyner.Server.admin
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDataService" in both code and config file together.
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        DomainStatistics[] GetDomainStatistics();

		[OperationContract]
		DomainStatistics[] GetDomainStatisticsByUser(User user);

        [OperationContract]
        PageRequest[] GetDomainRequests(Site site);

        [OperationContract]
        User LoginUser(User user);

		[OperationContract]
		List<LocationResult> GetLocations();
    }
}
