using BeIT.MemCached;
using System;

namespace DCT.Monitor.Cache
{
	[Serializable]
	public class Memcached : ICache
	{
		private MemcachedClient _Instance;

		public Memcached(string name, string[] servers)
		{
            try
            {
                _Instance = MemcachedClient.GetInstance(name);
            }
            catch
            {
                MemcachedClient.Setup(name, servers);
                _Instance = MemcachedClient.GetInstance(name);
            }
		}

		public T Get<T>(string key)
		{
            return (T)Get(key);
        }

        public object Get(string key){
			return _Instance.Get(key);
		}

		public bool Put(string key, object entity)
		{
			var cacheEntity = _Instance.Get(key);
			return (cacheEntity != null) ? _Instance.Replace(key, entity) : _Instance.Add(key, entity);
		}

        public bool Put(string key, object entity, DateTime expired)
        {
			var cacheEntity = _Instance.Get(key);
            return (cacheEntity != null) ? _Instance.Replace(key, entity, expired) : _Instance.Add(key, entity, expired);
        }
	}
}
