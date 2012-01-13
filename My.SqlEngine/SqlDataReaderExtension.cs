using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace My.SqlEngine
{
	public static class SqlDataReaderExtension
	{
		public static TObject GetField<TObject>(this SqlDataReader rdr, string name)
		{
			try
			{
				var index = rdr.GetOrdinal(name);
				var val = rdr.GetValue(index);
				if(val is TObject) return (TObject) val;
				return (TObject)Convert.ChangeType(val, typeof(TObject));
			}
			catch
			{
				return default(TObject);
			}
		}

		public static List<TObject> ReadList<TObject>(this SqlDataReader rdr, IDataLoader<TObject> loader)
			where TObject: new()
		{
			return ReadList(rdr, new List<TObject>(), loader);
		}


		public static List<TObject> ReadList<TObject>(this SqlDataReader rdr, List<TObject> target, IDataLoader<TObject> loader)
			where TObject : new()
		{
			while (rdr.Read())
			{
				target.Add(loader.LoadData(rdr));
			}
			return target;
		}
	}
}
