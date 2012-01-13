using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Inetgiant.Monitor.Entities;
using Inetgiant.Monitor.Modules;

namespace Inetgiant.Monitor.StreamInsight
{
    class MsSqlStatisticRepository : IStatisticRepository
    {
        const string connString = "Data Source=.;Initial Catalog=MonatorDb;Integrated Security=True";
        public void SaveStreamToDb(IEnumerable<PagePingEvent> StreamToSave)
        {
            using (var conn = new SqlConnection(connString))
            {
                foreach (var pingItem in StreamToSave)
                {
                    var cmd = new SqlCommand()
                    {
                        Connection = conn,
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText =
                            @"dbo.usp_InsertInfo"
                    };

                    cmd.Parameters.Add(new SqlParameter()
                    {
                        DbType = System.Data.DbType.String,
                        Value = pingItem.Domain,
                        ParameterName = "Domain"
                    });

                    cmd.Parameters.Add(new SqlParameter()
                    {
                        DbType = System.Data.DbType.String,
                        Value = pingItem.Url,
                        ParameterName = "Url"
                    });

                    cmd.Parameters.Add(new SqlParameter()
                    {
                        DbType = System.Data.DbType.String,
                        Value = pingItem.IpAddress,
                        ParameterName = "IpAdress"
                    });

                    cmd.Parameters.Add(new SqlParameter()
                    {
                        DbType = System.Data.DbType.Byte,
                        Value = pingItem.Browser,
                        ParameterName = "Browser"
                    });

                    cmd.Parameters.Add(new SqlParameter()
                    {
                        DbType = System.Data.DbType.Guid,
                        Value = pingItem.RequestIdentifier,
                        ParameterName = "ReqId"
                    });

                    cmd.Parameters.Add(new SqlParameter()
                    {
                        DbType = System.Data.DbType.Guid,
                        Value = pingItem.SessionIdentifier,
                        ParameterName = "SessId"
                    });

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
