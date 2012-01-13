using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCT.Monitor.Entities;

namespace Monitor.DAL
{
    public class RequestDummiRepository : DummiRepository<PageRequest, Guid>, IRequestRepository
    {
    }

    public class DummiRepository<T, T1> : IBaseRepository<T, T1>
        where T : class, IEntity<T1>
    {
        public void Create(T entity)
        {
            if (typeof(T1) == typeof(Guid))
            {
                object o = Guid.NewGuid();
                entity.Id = (T1)o;
            }
        }

        public void Update(T entity)
        {
        }

        public void Delete(T1 id)
        {
        }

        public void Delete(T entity)
        {
        }

        public void Create(IEnumerable<T> entities)
        {
        }

        public void Update(IEnumerable<T> entities)
        {
        }

        public void Delete(IEnumerable<T> entities)
        {
        }

        public List<T> GetAll()
        {
            return null;
        }

        public T GetById(T1 id)
        {
            return null;
        }
    }
}
