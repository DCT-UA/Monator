using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DCT.Monitor.Entities;
using My.SqlEngine;
using System.Data.SqlClient;

namespace Monitor.DAL.Implementation.Pages
{
    public class MsSqlPageRepository : FullCachedRepository<Page, Guid>, IPageRepository
    {
        //Properties of entities
        public const string DomainId = "DomainId";
        public const string Page = "Page";
        public const string PageId = "Id";

        //Name actions
        public const string GetByDomainId = "GetByDomain";
        public const string GetbyId = "GetById";

        public class PageParser : IDataLoader<Page>
        {
            public Page LoadData(SqlDataReader reader, Page data)
            {
                if (data == null) data = new Page();
                
                data.Id = reader.GetField<Guid>("Id");
                data.DomainId = reader.GetField<Guid>(DomainId);
                data.PagePattern = reader.GetField<String>(Page);                

                return data;
            }
        }

        public MsSqlPageRepository()
            : base((i) => i.DomainId) // creates index
        {
        }

        protected override void OnInitUpdateSp(StoredProc proc, Page entity)
        {
            proc.SetParam(DomainId, SqlDbType.UniqueIdentifier, entity.DomainId);
            proc.SetParam(Page, SqlDbType.VarChar, 255, entity.PagePattern);            
        }

        protected override IDataLoader<Page> OnCreateLoader()
        {
            return new PageParser();
        }

        public List<Page> GetPagesBySiteId(Guid domainId)
        {
            //using (var sp = Sql.GetProc(GetProcName(GetByDomainId)))
            //{
            //    sp.SetParam(DomainId, SqlDbType.UniqueIdentifier, domainId);

            //    return sp.ExecuteList<Page>(DataLoader);
            //}
            List<Page> items;
            if (ForeignIndex.TryGetValue(domainId, out items))
            {
                return items.ToList(); // clone
            }

            return new List<Page>();
        }
    }
}
