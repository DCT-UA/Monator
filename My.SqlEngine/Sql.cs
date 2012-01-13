using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My.SqlEngine
{
	public class Sql
	{
		public ISqlConnector Connector { get; private set; }

		public Sql(ISqlConnector connector)
		{
			Connector = connector;
		}

		public StoredProc GetProc(string name)
		{
			return new StoredProc(this, name);
		}
	}
}
