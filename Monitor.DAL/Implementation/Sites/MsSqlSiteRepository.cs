using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DCT.Monitor.Entities;
using My.SqlEngine;
using System.Data.SqlClient;

namespace Monitor.DAL.Implementation.Sites
{
    public class MsSqlSiteRepository: FullCachedRepository<Site, Guid>, ISiteRepository
    {
		//Properties of entities
        public const string Domain = "Domain";
        public const string UserId = "UserId";
		public const string IgnoreSubdomains = "IgnoreSubdomains";

		//Name actions
		public const string GetByUserId = "GetByUser";
		public const string GetSiteByDomain = "GetByDomain";

        public class SiteParser : IDataLoader<Site>
        {
            public Site LoadData(SqlDataReader reader, Site data)
            {
                if (data == null) data = new Site();

                data.Id = reader.GetField<Guid>("Id");
                data.UserId = reader.GetField<Guid>(UserId);
                data.Domain = reader.GetField<String>(Domain);
				data.ContainsSubdomains = reader.GetField<bool>(IgnoreSubdomains);

                return data;
            }
        }

        protected override void OnInitUpdateSp(StoredProc proc, Site entity)
        {
			proc.SetParam(IgnoreSubdomains, SqlDbType.Bit, entity.ContainsSubdomains);
            proc.SetParam(Domain, SqlDbType.VarChar, entity.Domain);
            proc.SetParam(UserId, SqlDbType.UniqueIdentifier, entity.UserId);
			//proc.SetParam(IdParameter, SqlDbType.UniqueIdentifier, entity.Id);
        }

        protected override IDataLoader<Site> OnCreateLoader()
        {
            return new SiteParser();
        }

		public List<Site> GetSitesByUser(Guid userId)
		{
            return GetAll().Where(i => i.UserId == userId).ToList();
		}

		public Site GetSite(string domain)
        {
            return GetAll().FirstOrDefault(i => i.Domain == domain);
		}

		public List<Site> GetSites()
		{
			return base.GetAll();
		}
    }
}
