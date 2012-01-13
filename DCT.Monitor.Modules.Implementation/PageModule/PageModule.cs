using System;
using System.Collections.Generic;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using Monitor.DAL;
using DCT.Unity;

namespace DCT.Monitor.Modules.Implementation.PageModule
{
    public class PageModule : IPageModule
    {
        private IPageRepository _repository = ServiceLocator.Current.Resolve<IPageRepository>();

        public void CreatePage(Page page)
        {
            _repository.Create(page);
        }

        public List<Page> GetPagesBySiteId(Guid siteId)
        {
           return _repository.GetPagesBySiteId(siteId);
        }

        public void UpdatePage(Page page)
        {
            _repository.Update(page);
        }

        public void DeletePage(Page page)
        {
            _repository.Delete(page);
        }

        public Page GetPageById(Guid pageId)
        {
           return _repository.GetById(pageId);
        }
    }
}
