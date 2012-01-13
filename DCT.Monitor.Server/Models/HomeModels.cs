using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace DCT.Monitor.Server.Models
{
	#region Models
	public class ContactUsModel : ViewModel
	{
		[Required]
		[DataType(DataType.Text)]
		[ValidateEmail]
		[DisplayName("User email")]
		public string UserEmail { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[DisplayName("Message")]
		public string Message { get; set; }
	}

	#endregion

	#region Validation


	#endregion
}