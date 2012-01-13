using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inetgiant.Monitor.Entities;

namespace Skyner.Server.Models
{
    public class SiteListModel
    {
        public List<Site> Sites { get; set; }

		public SiteListModel()
		{

		}
    }
}