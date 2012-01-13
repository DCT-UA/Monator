using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules.Implementation.SiteManager;

namespace DCT.Monitor.Server.Models
{
	#region Models

	public class SiteListModel : ViewModel
    {
		private List<Site> siteList;
		public List<Site> Sites { get { return siteList; } set { siteList = value; } }
		public SiteModel SiteModel { get; set; }

		public SiteListModel()
		{
			SiteModel = new SiteModel();
		}

		public SiteListModel(SiteModel model)
		{
			SiteModel = model;
		}
	}

	[ValidateDuplicateDomain]
	public class SiteModel : ViewModel
	{
		public Guid Id { get; set; }

		[Required]
		[StringLength(int.MaxValue)]
		[DataType(DataType.Text)]
		[DisplayName("Domain")]
		[RegularExpression("([\\w-*]+\\.)+[\\w-]+", ErrorMessage = "The domain name provided is invalid. Please check the value and try again.")]
		public string Domain { get; set; }

		public bool IgnoreSubdomains { get; set; }
	}

	#endregion

	#region Validation

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class ValidateDuplicateDomainAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "Domain already exists.";
		private SiteManagerModule _siteModule = new SiteManagerModule();


		public ValidateDuplicateDomainAttribute()
			: base(_defaultErrorMessage)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString);
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return false;

			var domain = (value as SiteModel).Domain;

			List<Site> collection = _siteModule.GetSites();
			foreach (Site site in collection)
			{
				if (domain == site.Domain)
					return false;
			}
			return true;
		}
	}

	#endregion
}