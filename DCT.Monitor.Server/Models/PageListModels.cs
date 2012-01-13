using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Server.Models
{
    public class PageListModel: ViewModel
    {
		private List<Page> pageList;
		public List<Page> Pages { get { return pageList; } set { pageList = value; } }
		public PageModel PageModel { get; set; }
        public Site Site { get; set; }

		public PageListModel()
		{
			PageModel = new PageModel();
		}

        public PageListModel(PageModel model)
		{
			PageModel = model;
		}
    }

    [ValidateDuplicateDomain]
	public class PageModel : ViewModel
	{
		public Guid Id { get; set; }

        public Guid SiteId{get; set;}

		[Required]
		[StringLength(int.MaxValue)]
		[DataType(DataType.Text)]
		[DisplayName("PageUrl")]
		[RegularExpression("([\\w-]+\\.)+[\\w-]+", ErrorMessage = "The domain name provided is invalid. Please check the value and try again.")]
		public string Page { get; set; }		
	}
}