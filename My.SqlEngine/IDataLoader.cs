using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace My.SqlEngine
{
	public interface IDataLoader<TData>
	{
		TData LoadData(SqlDataReader reader, TData data);
	}

    public static class IDataLoaderExtensions
    {
        public static TData LoadData<TData>(this IDataLoader<TData> _this, SqlDataReader reader)
             where TData : new()
        {
            return _this.LoadData(reader, new TData());
        }
    }
}
