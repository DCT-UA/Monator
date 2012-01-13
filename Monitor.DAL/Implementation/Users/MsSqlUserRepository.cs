using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DCT.Monitor.Entities;
using My.SqlEngine;

namespace Monitor.DAL.Implementation.Users
{
    public class MsSqlUserRepository: FullCachedRepository<User, Guid>, IUserRepository
    {
        public class UserParser : IDataLoader<User>
        {
            public User LoadData(System.Data.SqlClient.SqlDataReader reader, User data)
            {
                if (data == null) data = new User();

                data.Id = reader.GetField<Guid>("Id");
                data.UserName = reader.GetField<string>(UserName);
                data.Email = reader.GetField<string>(Email);
				data.ProviderId = reader.GetField<int>(ProviderId);
				data.ProviderUserId = reader.GetField<string>(ProviderUserId);
                data.Role = reader.GetField<string>(Role);
                data.Password = reader.GetField<string>(Password);
                return data;
            }
        }

        public const string UserName = "UserName";
        public const string Password = "Password";
        public const string Email = "Email";
		public const string ProviderId = "ProviderId";
        public const string ProviderUserId = "ProviderUserId";
        public const string Role = "Role";

        public const string ValidateAction = "Validate";
		public const string GetByName = "GetUserByName";
		public const string GetByProviderUserId = "GetUserByProviderUserId";
		public const string GetByEmail = "GetUserByEmail";
		public const string ValidatePassword = "ValidatePassword";
		public const string ChangeCurrentPassword = "ChangePassword";
		public const string RestoreUserPassword = "RestorePassword";
		public const string BindProvider = "AddProvider";
		public const string GetUserEmail = "GetEmail";
		public const string GetUserId = "GetIdByUserName";
		public const string DeleteProvider = "DeleteProvider";

		public const string OldPassword = "OldPassword";
		public const string NewPassword = "NewPassword";
		public const string CheckPassword = "CheckPassword";

        protected override void OnInitUpdateSp(StoredProc sp, User user)
        {
            sp.SetParam(UserName, SqlDbType.NVarChar, 50, user.UserName);
            sp.SetParam(Password, SqlDbType.NVarChar, 300, user.Password);
            sp.SetParam(Email, SqlDbType.NVarChar, 200, user.Email);
			sp.SetParam(ProviderId, SqlDbType.Int, user.ProviderId);
			sp.SetParam(ProviderUserId, SqlDbType.NVarChar, 500, user.ProviderUserId);
        }

        protected override IDataLoader<User> OnCreateLoader()
        {
            return new UserParser();
        }

		//Authentificate user
        public bool Authenticate(User user)
        {
            if (!string.IsNullOrWhiteSpace(user.UserName))
            {
                var userToValidate = GetUser(user.UserName);

                return userToValidate != null && userToValidate.Password.Equals(user.Password) ? true : false;
            }
            else { throw new ArgumentNullException("UserName"); }
            
           
            //using (var sp = Sql.GetProc(GetProcName(ValidateAction)))
            //{
            //    sp.SetParam(UserName, SqlDbType.NVarChar, 50, user.UserName);
            //    sp.SetParam(Password, SqlDbType.NVarChar, 300, user.Password);

            //    sp.SetOutputParam(IdParameter, SqlDbType.UniqueIdentifier);

            //    sp.Execute();
            //    var id = sp[IdParameter];
            //    if (id == null || !(id is Guid)) return false;

            //    user.Id = (Guid)id;
            //    return true;
            //}
        }

		//Get user by username
		public User GetUser(string userName)
		{            
            var user = GetAll().Where(n=>n.UserName==userName).SingleOrDefault();
            return user;
		}

		//Get user by provider
		public User GetUserByProviderUserId(string providerUserId)
		{
            var user = GetAll().Where(n => n.ProviderUserId == providerUserId).SingleOrDefault();
            return user;
		}

		//Get user by your email
		public User GetUserByEmail(string email)
		{
            var user = GetAll().Where(n => n.Email == email).SingleOrDefault();
            return user;
		}

        public User GetUserById(Guid userId)
        {
            var user = GetAll().Where(n => n.Id == userId).SingleOrDefault();
            return user;
        }

		//Change password
		public bool ChangePassword(string oldPassword, string newPassword, string username)
		{
            var user = GetUser(username);

            if (user.Password.Equals(oldPassword))
            {
                user.Password = newPassword;
               
            }

            using (var sp = Sql.GetProc(GetProcName(ChangeCurrentPassword)))
            {
                sp.SetParam(IdParameter, SqlDbType.UniqueIdentifier, user.Id);
                sp.SetParam(OldPassword, SqlDbType.NVarChar, 300, oldPassword);
                sp.SetParam(NewPassword, SqlDbType.NVarChar, 300, newPassword);

                return sp.ExecuteScalar<bool>();
            }
		}

		//Change password
		public bool RestorePassword(string newPassword, Guid userId)
		{
            var user = GetAll().Where(n => n.Id == userId).SingleOrDefault();

            if (user != null)
            {
                user.Password = newPassword;                
            }
            

            using (var sp = Sql.GetProc(GetProcName(RestoreUserPassword)))
            {
                sp.SetParam(IdParameter, SqlDbType.UniqueIdentifier, userId);
                sp.SetParam(NewPassword, SqlDbType.NVarChar, 300, newPassword);

                return sp.ExecuteScalar<bool>();
            }
		}

		//Add bind to provider
		public bool AddProvider(int providerId, string providerUserId, Guid userId)
		{
            var user = GetAll().Where(n => n.Id == userId).SingleOrDefault();

            if (user != null)
            {
                user.ProviderId = providerId;
                user.ProviderUserId = providerUserId;
                
                return true;
            }
            return false;
            
            //using (var sp = Sql.GetProc(GetProcName(BindProvider)))
            //{
            //    sp.SetParam(ProviderId, SqlDbType.Int, providerId);
            //    sp.SetParam(ProviderUserId, SqlDbType.NVarChar, 500, providerUserId);
            //    sp.SetParam(IdParameter, SqlDbType.UniqueIdentifier, userId);

            //    return sp.ExecuteScalar<bool>();
            //}
		}

		//Get email
		public string GetEmail(string userName)
		{
            var email = GetAll().Where(n => n.UserName == userName).SingleOrDefault().Email;
            return email;
		}

		//Get id
		public Guid GetId(string userName)
		{
            var userId = GetAll().Where(n => n.UserName == userName).SingleOrDefault().Id;
            return userId;
		}

		public void CleanProvider(string providerUserId)
		{
            var user = GetAll().Where(n => n.ProviderUserId == providerUserId).SingleOrDefault();

            if (user != null)
            {
                user.ProviderId = 1;
                user.ProviderUserId = "";
            }
            //using (var sp = Sql.GetProc(GetProcName(DeleteProvider)))
            //{
            //    sp.SetParam(ProviderUserId, SqlDbType.NVarChar, 500, providerUserId);
            //    sp.ExecuteScalar<int>();
            //}
		}
    }
}
