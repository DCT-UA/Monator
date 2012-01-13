using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCT.Monitor.Entities;

namespace Monitor.DAL
{
    public interface IUserRepository: IBaseRepository<User, Guid>
    {
        bool Authenticate(User user);
		User GetUser(string userName);
		User GetUserByProviderUserId(string providerUserId);
		User GetUserByEmail(string email);		
		bool ChangePassword(string oldPassword, string newPassword, string userName);
		bool RestorePassword(string newPassword, Guid userId);
		bool AddProvider(int providerId, string userProviderId, Guid userId); // TODO: remove from here
		string GetEmail(string userName);
		Guid GetId(string userName);
		void CleanProvider(string providerUserId);
        User GetUserById(Guid userId);
    }
}
