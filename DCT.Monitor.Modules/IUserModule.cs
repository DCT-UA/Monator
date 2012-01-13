using System;
using System.Collections.Generic;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Modules
{
    public interface IUserModule
    {
        void Create(User user);
		User GetUser(string userName);
		User GetUserByProviderUserId(string providerUserId);
		User GetUserByEmail(string email);
        bool Authenticate(User user);
        void Update(User user);
        void Delete(User user);
		bool ChangePassword(string oldPassword, string newPassword, Guid userId);
		bool RestorePassword(string newPassword, Guid userId);
		bool AddProvider(int providerId, string providerUserId, Guid userId);
		string GetEmail(string userName);
		Guid GetId(string userName);
		string GeneratePassword();
		void CleanProvider(string providerUserId);
		List<User> GetAll();
    }
}
