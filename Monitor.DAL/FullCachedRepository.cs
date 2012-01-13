using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using DCT.Monitor.Entities;

namespace Monitor.DAL
{
    /// <summary>
    /// Base class for repositories with full cached data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public abstract class FullCachedRepository<T, T1> : SqlBaseRepository<T, T1>, ICacheRepository<T, T1>
        where T : class, IEntity<T1>, new() 
    {
        protected static Dictionary<T1, T> Cache { get; private set; }
        protected static ConcurrentDictionary<T1, List<T>> ForeignIndex { get; private set; }
        private Func<T, T1> _foreignKeyExpression;

        public FullCachedRepository()
            :this(null)
        {
        }

        public FullCachedRepository(Func<T, T1> fkExpression)
        {
            _foreignKeyExpression = fkExpression;
            Cache = Cache ?? base.OnGetAll().ToDictionary(i => i.Id, i => i);

            if(_foreignKeyExpression != null) CreateForeignIndex();
        }

        protected override List<T> OnGetAll()
        {
            return Cache.Values.ToList();
        }

        private void CreateForeignIndex()
        {
            var data = Cache.Values.ToLookup(i => _foreignKeyExpression(i), i => i).ToDictionary(i => i.Key, i => i.ToList());
            ForeignIndex = new ConcurrentDictionary<T1, List<T>>(data);
        }

        protected override T OnGetById(T1 Id)
        {
            T site;
            Cache.TryGetValue(Id, out site);
            return site;
        }

        protected override void OnCreate(T entity)
        {
            base.OnCreate(entity);
            Cache.Add(entity.Id, entity);

            if (_foreignKeyExpression != null)
            {
                var bug = ForeignIndex.GetOrAdd(_foreignKeyExpression(entity), (id) => new List<T>());
                bug.Add(entity);
            }
        }

        protected override void OnDelete(T1 Id)
        {
            var entity = GetById(Id);

            base.OnDelete(Id);
            Cache.Remove(Id);

            if (_foreignKeyExpression != null)
            {
                var list = ForeignIndex.GetOrAdd(_foreignKeyExpression(entity), (id) => new List<T>());
                list.Remove(entity);
            }
        }

        protected override void OnUpdate(T entity)
        {
            base.OnUpdate(entity);
            Cache[entity.Id] = entity;
        }

		public void Reset()
		{
			Cache = base.OnGetAll().ToDictionary(i => i.Id, i => i);
            if (_foreignKeyExpression != null) CreateForeignIndex();
		}
	}
}
