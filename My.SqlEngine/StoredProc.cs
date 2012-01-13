using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace My.SqlEngine
{
	public class StoredProc: IDisposable
	{
		public Sql Sql { get; set; }
		public SqlCommand Command { get; private set; }

        /// <summary>
        /// Creates instance of wrapper for stored procedure
        /// </summary>
        /// <param name="sql">Instance of sql engine</param>
        /// <param name="name">Name of stored procedure</param>
		public StoredProc(Sql sql, string name)
		{
			Sql = sql;
			Command = new SqlCommand(name);
			Command.CommandType = CommandType.StoredProcedure;
		}

        /// <summary>
        /// Gets or sets parameters of stored procedure
        /// </summary>
        /// <param name="name">Name of parameter</param>
		public object this[string name]
		{ 
			get
			{
				return Command.Parameters[name].Value;
			}
			set
			{
				SetParam(name, value);
			}
		}

        /// <summary>
        /// Sets parameter of stored procedure
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>created SqlParameter</returns>
        public SqlParameter SetParam(string name, object value)
		{
			if(value == null)
				Command.Parameters.Add(name, SqlDbType.NVarChar).IsNullable = true;
			else Command.Parameters.AddWithValue(name, value);

            return Command.Parameters[name];
		}

        /// <summary>
        /// Sets parameter of stored procedure using name, value and db type
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dataType">Parameter db type</param>
        /// <param name="value">Parameter value</param>
        /// <returns>created SqlParameter</returns>
        public SqlParameter SetParam(string name, SqlDbType dataType, object value)
		{
            var p = Command.Parameters.Add(name, dataType);
            p.Value = value;

            return p;
		}

        /// <summary>
        /// Sets parameter of stored procedure using name, value, size and db type
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dataType">Parameter db type</param>
        /// <param name="dataType">Parameter size</param>
        /// <param name="value">Parameter value</param>
        /// <returns>created SqlParameter</returns>
        public SqlParameter SetParam(string name, SqlDbType sqlDbType, int size, object value)
        {
            var p = SetParam(name, sqlDbType, value);
            p.Size = size;

            return p;
        }

        /// <summary>
        /// Creates output parameter of stored procedure using name and db type
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dataType">Parameter db type</param>
        /// <returns>created SqlParameter</returns>
        public SqlParameter SetOutputParam(string name, SqlDbType type)
		{
            var p = Command.Parameters.Add(name, type);
			p.Direction = ParameterDirection.Output;

            return p;
		}

        /// <summary>
        /// Creates reference parameter of stored procedure using name and value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>created SqlParameter</returns>
        public SqlParameter SetReference(string name, object value)
		{
            var p = SetParam(name, value);
			p.Direction = ParameterDirection.InputOutput;

            return p;
		}

        /// <summary>
        /// Creates reference parameter of stored procedure using name, value and db type
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="dataType">Parameter db type</param>
        /// <param name="value">Parameter value</param>
        /// <returns>created SqlParameter</returns>
        public SqlParameter SetReference(string name, SqlDbType dbType, object value)
        {
            var p = SetParam(name, dbType, value);
            p.Direction = ParameterDirection.InputOutput;

            return p;
        }

        /// <summary>
        /// Executes stored procedure as DataReader and reads data to list
        /// </summary>
        /// <typeparam name="TData">Type of data to retrive</typeparam>
        /// <param name="loader">Data loader to fetch data from reader</param>
        /// <returns>List of loaded data</returns>
		public List<TData> ExecuteList<TData>(IDataLoader<TData> loader) 
			where TData: new()
		{
			var list = new List<TData>();

            using (InitConnection())
            {
                using (var rdr = Command.ExecuteReader())
                {
                    if (rdr == null) return list;

                    rdr.ReadList(list, loader);
                }
            }

			return list;
		}

        /// <summary>
        /// Executes stored procedure as DataReader and reads first record
        /// </summary>
        /// <typeparam name="TData">Type of data to retrive</typeparam>
        /// <param name="loader">Data loader to fetch data from reader</param>
        /// <returns>loaded entity</returns>
        public TData ExecuteSingle<TData>(IDataLoader<TData> loader)
            where TData: new()
        {
            TData data = default(TData);

            using (InitConnection())
            {
                using (var rdr = Command.ExecuteReader())
                {
                    if (rdr == null || !rdr.Read()) return data;
                    data = loader.LoadData(rdr);
                }
            }

            return data;
        }

        /// <summary>
        /// Executes stored procedure as Scalar and returns obtained value
        /// </summary>
        /// <typeparam name="TObject">Type of value to return</typeparam>
        /// <returns>returned data</returns>
		public TObject ExecuteScalar<TObject>()
		{
            using (InitConnection())
            {
                var obj = Command.ExecuteScalar();
                if (obj == null) return default(TObject);
                if (obj is TObject) return (TObject)obj;
                return (TObject)Convert.ChangeType(obj, typeof(TObject));
            }
		}

        /// <summary>
        /// Executes stored procedure as non query
        /// </summary>
        /// <returns>Number of affected records</returns>
		public int Execute()
		{
            using (InitConnection())
            {
                return Command.ExecuteNonQuery();
            }
		}

        /// <summary>
        /// Executes stored procedure as DataReader and call handler to process reader
        /// </summary>
        /// <param name="handler">handler to process reader</param>
		public void ExecuteReader(Action<SqlDataReader> handler)
		{
            using (InitConnection())
            {
                using (var rdr = Command.ExecuteReader())
                {
                    handler(rdr);
                }
            }
		}

		private SqlConnection InitConnection()
		{
			var cnt = Sql.Connector.CreateConnection();
			Command.Connection = cnt;
			cnt.Open();
			return cnt;
		}

        public void Dispose()
        {
            if (Command != null) Command.Dispose();
            Command = null;
        }
    }
}
