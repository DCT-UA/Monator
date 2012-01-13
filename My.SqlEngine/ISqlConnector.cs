using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace My.SqlEngine
{
	public interface ISqlConnector
	{
		SqlConnection CreateConnection();
	}
}
