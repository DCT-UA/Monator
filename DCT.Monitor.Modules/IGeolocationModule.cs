using System.Collections.Generic;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Modules
{
	public interface IGeolocationModule
	{
		List<LocationResult> Convert(List<PageRequest> data);
	}
}
