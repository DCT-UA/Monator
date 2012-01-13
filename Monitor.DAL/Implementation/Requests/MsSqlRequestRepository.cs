using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DCT.Monitor.Entities;
using My.SqlEngine;
using DCT.Unity;
using System.Data;

namespace Monitor.DAL.Implementation.Requests
{
    public class MsSqlPageRequestRepository : SqlBaseRepository<PageRequest, Guid>, IRequestRepository
    {
        public class RequestParser : IDataLoader<PageRequest>
        {
            public PageRequest LoadData(SqlDataReader reader, PageRequest data)
            {
                throw new NotImplementedException();
            }
        }

        private const string DomainField = "Domain";
        private const string UrlField = "Url";
        private const string IpAddressField = "IpAddress";
        private const string BrowserField = "Browser";
        private const string SessionIdField = "SessionId";
        private const string ReffererField = "Refferer";
        private const string SiteIdField = "SiteId";
		private const string UserIdField = "UserId";

        protected override void OnInitUpdateSp(StoredProc proc, PageRequest entity)
        {
            proc.SetParam(DomainField, SqlDbType.NVarChar, 4000, entity.Domain);
            proc.SetParam(UrlField, SqlDbType.NVarChar, 4000, entity.Url);
            proc.SetParam(IpAddressField, SqlDbType.NVarChar, 50, entity.IpAddress);
            proc.SetParam(BrowserField, SqlDbType.SmallInt, entity.Browser);
            proc.SetParam(ReffererField, SqlDbType.NVarChar, 4000, entity.Refferer);
            proc.SetParam(SiteIdField, SqlDbType.UniqueIdentifier, entity.SiteId);
			proc.SetParam(UserIdField, SqlDbType.UniqueIdentifier, entity.UserId);
        }

        protected override IDataLoader<PageRequest> OnCreateLoader()
        {
            return new RequestParser();
        }
    }
}
