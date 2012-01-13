using System;

namespace DCT.Monitor.Cache
{
	public interface ICache<T>
	{
		T Get(string key);
		bool Put(string key, T entity);
		bool Put(string key, T entity, DateTime expired);
	}

    public interface ICache : ICache<object>
    {
        T Get<T>(string key);
    }
}
