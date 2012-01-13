using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.DAL
{
    /// <summary>
    /// Base Repository provides base functionality such as Update/Insert/Delete/Select
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public interface IBaseRepository<T, T1>
    {
        /// <summary>
        /// Create one entity
        /// </summary>
        /// <param name="entity">Entity object to create</param>
        void Create(T entity);

        /// <summary>
        /// Updates one entity
        /// </summary>
        /// <param name="entity">entity to perform update for</param>
        void Update(T entity);

        /// <summary>
        /// Deletes entity
        /// </summary>
        /// <param name="entity">entity to delete</param>
        void Delete(T1 id);

        /// <summary>
        /// Deletes entity by Id
        /// </summary>
        /// <param name="Id">Id of entity to delete</param>
        void Delete(T entity);

        /// <summary>
        /// Create set of entities
        /// </summary>
        /// <param name="entity">Entity set to create</param>
        void Create(IEnumerable<T> entities);

        /// <summary>
        /// Update set of entities
        /// </summary>
        /// <param name="entity">Entity set to update</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// Delete set of entities
        /// </summary>
        /// <param name="entity">Entity set to delete</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Gets list of all values
        /// </summary>
        /// <returns>List of all values</returns>
        List<T> GetAll();

        /// <summary>
        /// Get element by Id
        /// </summary>
        /// <param name="id">Gets element by Id</param>
        /// <returns>Element by Id</returns>
        T GetById(T1 id);
    }
}
