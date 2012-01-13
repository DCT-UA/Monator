using System;
using System.Collections.Generic;
using System.Linq;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using Monitor.DAL;
using DCT.Unity;

namespace DCT.Monitor.Modules.Implementation.GeolocationModule
{
	public class GeolocationModule: IGeolocationModule
	{
		public static IGeolocationRepository _repository = ServiceLocator.Current.Resolve<IGeolocationRepository>();
		public static Geolocation[] _geolocations = _repository.GetAll().ToArray();

		public List<LocationResult> Convert(List<PageRequest> data)
		{
			Dictionary<long, LocationResult> resultDictionary = new Dictionary<long, LocationResult>();
			foreach (var item in data)
			{
				Geolocation toSearch = new Geolocation();
				Geolocation location = BinarySearch(_geolocations, ConvertIP(item.IpAddress));
				if (location != null)
				{
					LocationResult gettedValue;
					bool returned = resultDictionary.TryGetValue(location.IpMin, out gettedValue);
					if (returned)
					{
						gettedValue.Count++;
					}
					else
					{
						resultDictionary.Add(location.IpMin, new LocationResult{ Latitude = location.Latitude, Longitude =location.Longitude, Count = 1 });
					}
				}
			}
			return resultDictionary.Values.ToList();
		}
	


		private Geolocation BinarySearch(Geolocation[] locationArray, long ip)
		{
			int low = 0;
			int high = locationArray.Length - 1;
			int mid;

			while (low <= high)
			{
				mid = (low + high) / 2;
				if (locationArray[mid].CompareTo(ip) < 0)
					low = mid + 1;
				else if (locationArray[mid].CompareTo(ip) > 0)
					high = mid - 1;
				else
					return locationArray[mid];
			}
			return null;
		}
		
		private long ConvertIP(string ip)
		{
			string[] parts = ip.Split('.');
			return ((Int64.Parse(parts[0])*256+Int64.Parse(parts[1]))*256+Int64.Parse(parts[2]))*256 + Int64.Parse(parts[3]);
		}
	}
}
