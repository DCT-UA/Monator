using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DCT.Monitor.Entities;
using My.SqlEngine;
using System.Data.SqlClient;

namespace Monitor.DAL.Implementation.Providers
{
	public class MsSqlProviderRepository : FullCachedRepository<Provider, int>, IProviderRepository
	{
		public const string ProviderId = "ProviderId";
		public const string ProviderName = "ProviderName";
		public const string ValidateAction = "Validate";
		public const string GetProviderByName = "GetProviderByName";
		public const string GetProviderById = "GetProviderById";

		public class ProviderParser : IDataLoader<Provider>
		{
			public Provider LoadData(SqlDataReader reader, Provider data)
			{
				if (data == null) data = new Provider();

				data.Id = reader.GetField<int>(IdParameter);
				data.Name = reader.GetField<string>(ProviderName);

				return data;
			}
		}

		protected override void OnInitUpdateSp(StoredProc proc, Provider provider)
		{
			proc.SetParam(ProviderName, SqlDbType.NVarChar, 50, provider.Name);
			proc.SetParam(ProviderName, SqlDbType.Int, provider.Id);
		}

		protected override IDataLoader<Provider> OnCreateLoader()
		{
			return new ProviderParser();
		}

		public Provider GetProvider(string providerName)
		{
            return base.GetAll().First(i => i.Name == providerName);
		}
	}
}
