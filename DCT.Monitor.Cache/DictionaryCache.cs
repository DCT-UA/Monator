using System;
using System.Collections.Concurrent;

namespace DCT.Monitor.Cache
{
    public class DictionaryCache: ICache
    {
        private static ConcurrentDictionary<string, DictionaryRecord> Cache { get; set; }

        private class DictionaryRecord
        {
            public DictionaryRecord(object value, DateTime date)
            {
                Value = value;
                Date = date;
            }

            public DateTime Date { get; set; }
            public object Value { get; set; }
        }

        static DictionaryCache()
        {
            Cache = new ConcurrentDictionary<string, DictionaryRecord>();
        }

        public T Get<T>(string key)
        {
            var data = Get(key);
            return data == null ? default(T) : (T)data;
        }

        public object Get(string key)
        {
            DictionaryRecord record;
            if (!Cache.TryGetValue(key, out record)) return null;
            return record != null && DateTime.Now <= record.Date ? record.Value : null;
        }

        public bool Put(string key, object entity)
        {
            return Put(key, entity, DateTime.MaxValue);
        }

        public bool Put(string key, object entity, DateTime expired)
        {
            var record = new DictionaryRecord(entity, expired);
            return Cache.AddOrUpdate(key, record, (k, o) => record) == record;
        }
    }
}
