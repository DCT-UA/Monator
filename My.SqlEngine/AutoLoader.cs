using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace My.SqlEngine
{
	public class AutoLoader<TData>: IDataLoader<TData> where TData: class, new()
	{
		private SqlDataReader _rdr;
		private readonly PropertyInfo[] _properties;
		private Dictionary<PropertyInfo, string> _map;
		private readonly Dictionary<PropertyInfo, MethodInfo> _getFieldMap;
		private readonly MethodInfo _getFieldMethod;

		public AutoLoader()
		{
			var sqlextype = typeof (SqlDataReaderExtension);
			_properties = typeof (TData).GetProperties();
			_getFieldMethod = sqlextype.GetMethod("GetField");
			_getFieldMap = _properties.ToDictionary(p => p, p => _getFieldMethod.MakeGenericMethod(p.PropertyType));
		}

		public TData LoadData(SqlDataReader reader, TData data)
		{
			Init(reader);
			if(data == null) data = new TData();

			foreach (var pk in _map)
			{
				var prop = pk.Key;
				var fname = pk.Value;
				prop.SetValue(data, _getFieldMap[prop].Invoke(null, new object[]{reader, fname}), null);
			}
			return data;
		}

		private void Init(SqlDataReader rdr)
		{
			// avoid init on every load call
			if(rdr == _rdr) return;

			_rdr = rdr;
			var cnames = new string[rdr.FieldCount];
			for(int i = 0; i < cnames.Length; i++)
			{
				cnames[i] = rdr.GetName(i);
			}
			_map = _properties.Where(p => cnames.Contains(p.Name)).ToDictionary(prop => prop, prop => prop.Name);
		}
	}
}
