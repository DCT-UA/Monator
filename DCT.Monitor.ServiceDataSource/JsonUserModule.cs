using System;
using System.Collections.Generic;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;

namespace DCT.Monitor.ServiceDataSource
{
    public class JsonUserModule: IUserModule
    {
        private JsonDataServiceClient _client = new JsonDataServiceClient();

        public void Create(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string userName)
        {
            throw new NotImplementedException();
        }

        public User GetUserByProviderUserId(string providerUserId)
        {
            throw new NotImplementedException();
        }

        public User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool Authenticate(User user)
        {
            return _client.Authorize(user.UserName, user.Password) == "ok";
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public bool ValidCurrentPassword(string password, Guid userId)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(string oldPassword, string newPassword, Guid userId)
        {
            throw new NotImplementedException();
        }

        public bool RestorePassword(string newPassword, Guid userId)
        {
            throw new NotImplementedException();
        }

        public bool AddProvider(int providerId, string providerUserId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public string GetEmail(string userName)
        {
            throw new NotImplementedException();
        }

        public Guid GetId(string userName)
        {
            throw new NotImplementedException();
        }

        public string GeneratePassword()
        {
            throw new NotImplementedException();
        }

        public void CleanProvider(string providerUserId)
        {
            throw new NotImplementedException();
        }

		public List<User> GetAll()
		{
			throw new NotImplementedException();
		}
    }
}
