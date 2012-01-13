using System;

namespace DCT.Monitor.Cache
{
	public class CacheFactory<T>
	{
		public ICache<T> Create(Providers provider)
		{
			switch (provider)
			{
				case Providers.Memcached:
					//return new Memcached<T>();
				default:
					throw new ApplicationException("No implementation for such provider");
			}
		}
	}
}
