using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCT.Monitor.Entities;
using My.SqlEngine;
using System.Data.SqlClient;
using DCT.Unity;
using System.Data;
using DCT.Utils;

namespace Monitor.DAL
{
    /// <summary>
    /// Base Repository provides base functionality such as Update/Insert/Delete/Select
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="T1"></typeparam>
    public abstract class SqlBaseRepository<T, T1> : IBaseRepository<T, T1>
        where T: class, IEntity<T1>, new()
    {
        protected static string TypeName;
        protected static SqlDbType IdDbType;

        static SqlBaseRepository()
        {
            TypeName = typeof(T).Name;
            IdDbType = typeof(T1) == typeof(int) ? SqlDbType.Int : SqlDbType.UniqueIdentifier;
        }

        protected const string CreateAction = "Create";
        protected const string UpdateAction = "Update";
        protected const string GetAllAction = "GetAll";
        protected const string GetByIdAction = "GetById";
        protected const string DeleteAction = "Delete";

        protected const string IdParameter = "Id";

        protected Sql Sql { get; set; }
        protected IDataLoader<T> DataLoader { get; private set; }

        public SqlBaseRepository()
        {
            Sql = new Sql(ServiceLocator.Current.Resolve<ISqlConnector>());
            DataLoader = OnCreateLoader();
        }

        /// <summary>
        /// Create one entity
        /// </summary>
        /// <param name="entity">Entity object to create</param>
        protected virtual void OnCreate(T entity)
        {
            var proc = Sql.GetProc(GetProcName(CreateAction));

            OnInitUpdateSp(proc, entity);

            proc.SetOutputParam(IdParameter, SqlDbType.UniqueIdentifier);
            proc.Execute();

            entity.Id = (T1)proc[IdParameter];
        }

        /// <summary>
        /// Updates one entity
        /// </summary>
        /// <param name="entity">entity to perform update for</param>
        protected virtual void OnUpdate(T entity)
        {
            var proc = Sql.GetProc(GetProcName(UpdateAction));

            proc.SetParam(IdParameter, IdDbType, entity.Id);
            OnInitUpdateSp(proc, entity);

            proc.Execute();
        }

        /// <summary>
        /// Deletes entity by Id
        /// </summary>
        /// <param name="Id">Id of entity to delete</param>
        protected virtual void OnDelete(T1 Id)
        {
            var proc = Sql.GetProc(GetProcName(DeleteAction));

            proc.SetParam(IdParameter, IdDbType, Id);
            proc.Execute();
        }

        /// <summary>
        /// Get all entities
        /// </summary>
        protected virtual List<T> OnGetAll()
        {
            var proc = Sql.GetProc(GetProcName(GetAllAction));
            return proc.ExecuteList(DataLoader);
        }

        protected virtual T OnGetById(T1 Id)
        {
            var proc = Sql.GetProc(GetProcName(GetByIdAction));
            return proc.ExecuteSingle(DataLoader);
        }

        /// <summary>
        /// In derived class initialize parameters of stored procedure to perform update or insert
        /// </summary>
        /// <param name="proc">stored procedure instance</param>
        /// <param name="entity">entity to get values from</param>
        protected abstract void OnInitUpdateSp(StoredProc proc, T entity);

        /// <summary>
        /// In derived classes create IDataLoader to fetch results from DataReader
        /// </summary>
        /// <returns>Instance of data loader</returns>
        protected abstract IDataLoader<T> OnCreateLoader();

        /// <summary>
        /// Get standard proc name for some action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected string GetProcName(string action)
        {
            return string.Format("usp_{0}_{1}", TypeName, action);
        }

        // Implementation of IBaseRepository
        #region IBaseRepository

        public void Create(T entity)
        {
            OnCreate(entity);
        }

        public void Update(T entity)
        {
            OnUpdate(entity);
        }

        public void Delete(T1 id)
        {
            OnDelete(id);
        }

        public void Delete(T entity)
        {
            OnDelete(entity.Id);
        }

        public void Create(IEnumerable<T> entities)
        {
            entities.Each(OnCreate);
        }

        public void Update(IEnumerable<T> entities)
        {
            entities.Each(OnUpdate);
        }

        public void Delete(IEnumerable<T> entities)
        {
            entities.Each(Delete);
        }

        public List<T> GetAll()
        {
            return OnGetAll();
        }

        public T GetById(T1 id)
        {
            return OnGetById(id);
        }

        #endregion

    }

    public class SqlConnector: ISqlConnector
    {
        public string ConnectionString { get; set; }

        public SqlConnector(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
