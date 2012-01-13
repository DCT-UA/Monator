using System;
using System.Collections.Generic;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Modules
{
    public interface IPageModule
    {
        void CreatePage(Page page);
        List<Page> GetPagesBySiteId(Guid domainId);
        void UpdatePage(Page page);
        void DeletePage(Page page);
        Page GetPageById(Guid pageId);
    }
}
