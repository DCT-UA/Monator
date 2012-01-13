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
using DCT.Monitor.Server.Helpers;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules.Implementation.UserModule;

namespace DCT.Monitor.Server.Models
{
	#region Models

	public class ViewModel
	{
		public SessionHelper Session { get; set; }

		public ViewModel(SessionHelper session)
		{
			this.Session = session;
		}

		public ViewModel()
		{
		}
	}

	//[PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
	public class ChangePasswordModel : ViewModel
	{
		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Current password")]
		public string OldPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("New password")]
		[ValidatePasswordLength]
		public string NewPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[DisplayName("Confirm new password")]
		[ValidatePasswordLength]
		[Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	[ValidateLogonAttribute("UserName", "Password", ErrorMessage = "The login or password provided is invalid. Please enter a valid credentials values.")]
	public class LogOnModel
	{
		[Required]
		[StringLength(50)]
		[DisplayName("User name")]
		public string UserName { get; set; }

		[Required]
		[StringLength(300)]
		[DataType(DataType.Password)]
		[DisplayName("Password")]
		public string Password { get; set; }

		[DisplayName("Remember me?")]
		public bool RememberMe { get; set; }
	}

	public class RegisterModel
	{
		[Required]
		[StringLength(50)]
		[ValidateDuplicateUserName]
		[DisplayName("User name")]
		public string UserName { get; set; }

		[Required]
		[StringLength(200)]
		[DataType(DataType.EmailAddress)]
		[ValidateDuplicateUserEmail]
		[ValidateEmail]
		[DisplayName("Email address")]
		public string Email { get; set; }

		[Required]
		[StringLength(300)]
		[ValidatePasswordLength]
		[DataType(DataType.Password)]
		[DisplayName("Password")]
		public string Password { get; set; }

		[Required]
		[StringLength(300)]
		[ValidatePasswordLength]
		[DataType(DataType.Password)]
		[DisplayName("Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

    public class AccountModel
    {
        public LogOnModel LogonModel { get; set; }
        public RegisterModel RegisterModel { get; set; }

        public bool IsLogon { get; set; }

        public AccountModel()     
        {
        }

        public AccountModel(LogOnModel model)
        {
            LogonModel = model;
            RegisterModel = new RegisterModel();
            IsLogon = true;
        }

        public AccountModel(RegisterModel model)
        {
            LogonModel = new LogOnModel();
            RegisterModel = model;
            IsLogon = false;
        }
    }

	//temporary solution for validation
	public class RegisterModelContainer
	{
		public RegisterModel RegisterModel { get; set; }

		public RegisterModelContainer()
		{
		}

		public RegisterModelContainer(RegisterModel model)
		{
			RegisterModel = model;
		}
	}

	public class BindingModel : ViewModel
	{
		public AccountBindingModel AccountBinding{ get; set; }
        public ChangeBindingModel ChangeBinding{ get; set; }

        public BindingModel()     
        {
			this.AccountBinding = new AccountBindingModel();
			this.ChangeBinding = new ChangeBindingModel();
        }

		public BindingModel(AccountBindingModel model)
        {
			AccountBinding = model;
			ChangeBinding = new ChangeBindingModel();
        }

		public BindingModel(ChangeBindingModel model)
        {
			AccountBinding = new AccountBindingModel();
			ChangeBinding = model;
        }
	}

	public class AccountBindingModel : ViewModel
	{
		[Required]
		[DataType(DataType.Text)]
		[DisplayName("ProviderId")]
		public int ProviderId { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[DisplayName("ProviderUserId")]
		public string ProviderUserId { get; set; }
	}

	//Model for change binding (in bindUser - binding clean and write current user)
	public class ChangeBindingModel : ViewModel
	{
		public User CurrentUser { get; set; }
		public User BindUser { get; set; }

		public ChangeBindingModel()
		{
		}

		public ChangeBindingModel(User current, User bindUser)
		{
			CurrentUser = current;
			BindUser = bindUser;
		}
	}

	public class ForgotPasswordModel
	{
		[Required]
		[StringLength(50)]
		[ValidateUserNameAttribute]
		[DisplayName("User name")]
		public string UserName { get; set; }
	}

	

	#endregion

	#region Services
	// The FormsAuthentication type is sealed and contains static members, so it is difficult to
	// unit test code that calls its members. The interface and helper class below demonstrate
	// how to create an abstract wrapper around such a type in order to make the AccountController
	// code unit testable.

	public interface IAccountService
	{
		int MinPasswordLength { get; }

		//bool ValidateUser(string userName, string password);
		//MembershipCreateStatus CreateUser(string userName, string password, string email);

		//bool ChangePassword(string userName, string oldPassword, string newPassword);
	}

	public class AccountService : IAccountService
	{
		#region Constants

		private const int MIN_PASSWORD_LENGTH = 7;

		#endregion

		//private readonly MembershipProvider _provider;

		public AccountService()
		{
		}

		//public AccountService(MembershipProvider provider)
		//{
		//    _provider = provider ?? Membership.Provider;
		//}

		public int MinPasswordLength
		{
			get
			{
				return MIN_PASSWORD_LENGTH;
			}
		}

		//public bool ValidateUser(string userName, string password)
		//{
		//    if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
		//    if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

		//    return _provider.ValidateUser(userName, password);
		//}

		//public MembershipCreateStatus CreateUser(string userName, string password, string email)
		//{
		//    if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
		//    if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
		//    if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

		//    MembershipCreateStatus status;
		//    _provider.CreateUser(userName, password, email, null, null, true, null, out status);
		//    return status;
		//}

		//public bool ChangePassword(string userName, string oldPassword, string newPassword)
		//{
		//    if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
		//    if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
		//    if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

			// The underlying ChangePassword() will throw an exception rather
			// than return false in certain failure scenarios.
			//try
			//{
			//    MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
			//    return currentUser.ChangePassword(oldPassword, newPassword);
			//}
			//catch (ArgumentException)
			//{
			//    return false;
			//}
			//catch (MembershipPasswordException)
			//{
			//    return false;
			//}


	//    }
	}

	public interface IFormsAuthenticationService
	{
		void SignIn(string userName, bool createPersistentCookie);
		void SignOut();
	}

	public class FormsAuthenticationService : IFormsAuthenticationService
	{
		public void SignIn(string userName, bool createPersistentCookie)
		{
			if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

			FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
		}

		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}
	}
	#endregion

	#region Validation
	//public static class AccountValidation
	//{
	//    public static string ErrorCodeToString(MembershipCreateStatus createStatus)
	//    {
	//        // See http://go.microsoft.com/fwlink/?LinkID=177550 for
	//        // a full list of status codes.
	//        switch (createStatus)
	//        {
	//            case MembershipCreateStatus.DuplicateUserName:
	//                return "Username already exists. Please enter a different user name.";

	//            case MembershipCreateStatus.DuplicateEmail:
	//                return "A username for that e-mail address already exists. Please enter a different e-mail address.";

	//            case MembershipCreateStatus.InvalidPassword:
	//                return "The password provided is invalid. Please enter a valid password value.";

	//            case MembershipCreateStatus.InvalidEmail:
	//                return "The e-mail address provided is invalid. Please check the value and try again.";

	//            case MembershipCreateStatus.InvalidAnswer:
	//                return "The password retrieval answer provided is invalid. Please check the value and try again.";

	//            case MembershipCreateStatus.InvalidQuestion:
	//                return "The password retrieval question provided is invalid. Please check the value and try again.";

	//            case MembershipCreateStatus.InvalidUserName:
	//                return "The user name provided is invalid. Please check the value and try again.";

	//            case MembershipCreateStatus.ProviderError:
	//                return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

	//            case MembershipCreateStatus.UserRejected:
	//                return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

	//            default:
	//                return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
	//        }
	//    }
	//}
	#region empty
	#endregion
	//[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	//public sealed class PropertiesMustMatchAttribute : ValidationAttribute
	//{
	//    private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
	//    private readonly object _typeId = new object();

	//    public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
	//        : base(_defaultErrorMessage)
	//    {
	//        OriginalProperty = originalProperty;
	//        ConfirmProperty = confirmProperty;
	//    }

	//    public string ConfirmProperty { get; private set; }
	//    public string OriginalProperty { get; private set; }

	//    public override object TypeId
	//    {
	//        get
	//        {
	//            return _typeId;
	//        }
	//    }

	//    public override string FormatErrorMessage(string name)
	//    {
	//        return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
	//                OriginalProperty, ConfirmProperty);
	//    }

	//    public override bool IsValid(object value)
	//    {
	//        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
	//        object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
	//        object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
	//        return Object.Equals(originalValue, confirmValue);
	//    }
	//}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
		//AccountService accountService = new AccountService();
		private readonly int _minCharacters = 7;/*accountService.MinPasswordLenth;*/

		public ValidatePasswordLengthAttribute()
			: base(_defaultErrorMessage)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
					name, _minCharacters);
		}

		public override bool IsValid(object value)
		{
			string valueAsString = value as string;
			return (valueAsString != null && valueAsString.Length >= _minCharacters);
		}
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ValidateDuplicateUserNameAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "Username already exists. Please enter a different user name.";
		private UserModule _userModule = new UserModule();

		public ValidateDuplicateUserNameAttribute()
			: base(_defaultErrorMessage)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
					name);
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return false;
			string userName = value as string;
			var duplicate = _userModule.GetUser(userName);

			if (duplicate == null)
				return true;
			else
				return false;
		}
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ValidateDuplicateUserEmailAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "A username for that e-mail address already exists. Please enter a different e-mail address.";
		private UserModule _userModule = new UserModule();

		public ValidateDuplicateUserEmailAttribute()
			: base(_defaultErrorMessage)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
					name);
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return false;
			string userEmail = value as string;
			var duplicate = _userModule.GetUserByEmail(userEmail);

			if (duplicate == null)
				return true;
			else
				return false;
		}
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ValidateEmailAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "The e-mail address provided is invalid. Please check the value and try again.";

		public ValidateEmailAttribute()
			: base(_defaultErrorMessage)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name);
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return false;
			string userEmail = value as string;

			if (!Regex.IsMatch(userEmail, @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?"))
				return false;
			else
				return true;
		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class ValidateLogonAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "The login or password provided is invalid. Please enter a valid credentials.";
		private UserModule _userModule = new UserModule();

		public string UserName { get; set; }
		public string Password { get; set; }

		public ValidateLogonAttribute(string userName, string password)
			: base(_defaultErrorMessage)
		{
			UserName = userName;
			Password = password;
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, UserName, Password);
		}

		public override bool IsValid(object value)
	    {
	        if (value == null)
	            return false;

			string userName = (value as LogOnModel).UserName;
			string password = (value as LogOnModel).Password;

			if (userName == null || password == null)
				return false;

			var check = _userModule.Authenticate(new User() { UserName = userName, Password = password });
			return check;
	    }
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class ValidateUserNameAttribute : ValidationAttribute
	{
		private const string _defaultErrorMessage = "The user name provided is invalid. Please check the value and try again.";
		private UserModule _userModule = new UserModule();

		public ValidateUserNameAttribute()
			: base(_defaultErrorMessage)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name);
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return false;
			string name = value as string;
			var userEmail = _userModule.GetEmail(name);
			return userEmail != null ? true : false;
		}
	}

	#endregion

}
