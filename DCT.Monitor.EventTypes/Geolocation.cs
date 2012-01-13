using System;

namespace DCT.Monitor.Entities
{
	public class Geolocation : IEntity<int>, IComparable
	{
        public int Id { get { return 0; } set { } }

        public long IpMin { get; set; }
        public long IpMax { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
		
		public int CompareTo(object obj)
		{
			var result = 0;
			var location = (long)obj;
			if (IpMax < location)
			{
				result = -1;
			}
			else if (IpMin > location)
			{
				result = 1;
			}
			return result;
		}
	}
}
