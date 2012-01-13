using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Server.Models
{
	public class AdminModels : ViewModel
	{

	}

	public class AdminUserListModel : ViewModel
	{
		private List<User> userList;
		private List<string> siteListString;
		public UserModel UserModel { get; set; }

		public List<User> Users { get { return userList; } set { userList = value; } }
		public List<string> UserSitesString { get { return siteListString; } set { siteListString = value; } }

		public AdminUserListModel()
		{
			UserModel = new UserModel();
		}

		public AdminUserListModel(UserModel model)
		{
			UserModel = model;
		}
	}

	public class UserModel : ViewModel
	{
		public Guid Id { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[DisplayName("User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[DisplayName("User email")]
		public string UserEmail { get; set; }
	}
}