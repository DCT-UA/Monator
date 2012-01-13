using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCT.Monitor.Entities;

namespace Monitor.DAL
{
    public interface IPageRepository : IBaseRepository<Page, Guid>
    {
        List<Page> GetPagesBySiteId(Guid domainId);
        Page GetById(Guid pageId);
    }
}
