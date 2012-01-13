using System;
using System.Collections.Generic;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using Monitor.DAL;
using DCT.Unity;
using DCT.Utils;
using System.Security.Cryptography;

namespace DCT.Monitor.Modules.Implementation.UserModule
{
    public class UserModule: IUserModule
    {
		private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
		private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
		private static string PASSWORD_CHARS_NUMERIC = "23456789";
		private static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

        private static IUserRepository _repository = ServiceLocator.Current.Resolve<IUserRepository>();

        public void Create(User user)
        {
            _repository.Create(user);
        }

		public void MakeChange()
		{

		}

        public bool Authenticate(User user)
        {
            user.Password = user.Password.HashString();
            return _repository.Authenticate(user);
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

		public User GetUser(string userName)
		{
			return _repository.GetUser(userName);
		}

		public User GetUserByProviderUserId(string providerUserId)
		{
			return _repository.GetUserByProviderUserId(providerUserId);
		}

		public User GetUserByEmail(string email)
		{
			return _repository.GetUserByEmail(email);
		}

		public bool ChangePassword(string oldPassword, string newPassword, Guid userId)
		{
            var username = _repository.GetUserById(userId).UserName;

			return _repository.ChangePassword(oldPassword, newPassword, username);
		}

		public bool RestorePassword(string newPassword, Guid userId)
		{
			return _repository.RestorePassword(newPassword, userId);
		}

		public bool AddProvider(int providerId, string providerUserId, Guid userId)
		{
			return _repository.AddProvider(providerId, providerUserId, userId);
		}

		public string GetEmail(string userName)
		{
			return _repository.GetEmail(userName);
		}

		public Guid GetId(string userName)
		{
			return _repository.GetId(userName);
		}

		public string GeneratePassword()
		{
			int minLength = 8;
			int maxLength = 12;
			
			if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
				return null;

			char[][] charGroups = new char[][] 
			{
				PASSWORD_CHARS_LCASE.ToCharArray(),
				PASSWORD_CHARS_UCASE.ToCharArray(),
				PASSWORD_CHARS_NUMERIC.ToCharArray(),
				PASSWORD_CHARS_SPECIAL.ToCharArray()
			};

			int[] charsLeftInGroup = new int[charGroups.Length];

			for (int i=0; i<charsLeftInGroup.Length; i++)
				charsLeftInGroup[i] = charGroups[i].Length;
        
			int[] leftGroupsOrder = new int[charGroups.Length];

			for (int i=0; i<leftGroupsOrder.Length; i++)
				leftGroupsOrder[i] = i;

			byte[] randomBytes = new byte[4];

			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(randomBytes);

			int seed = (randomBytes[0] & 0x7f) << 24 |
						randomBytes[1]         << 16 |
						randomBytes[2]         <<  8 |
						randomBytes[3];

			Random  random  = new Random(seed);

			char[] password = null;

			if (minLength < maxLength)
				password = new char[random.Next(minLength, maxLength+1)];
			else
				password = new char[minLength];

			int nextCharIdx;
			int nextGroupIdx;
			int nextLeftGroupsOrderIdx;
			int lastCharIdx;
			int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
			for (int i=0; i<password.Length; i++)
			{
				if (lastLeftGroupsOrderIdx == 0)
					nextLeftGroupsOrderIdx = 0;
				else
					nextLeftGroupsOrderIdx = random.Next(0, 
															lastLeftGroupsOrderIdx);

				nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];
				lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;
				if (lastCharIdx == 0)
					nextCharIdx = 0;
				else
					nextCharIdx = random.Next(0, lastCharIdx+1);

				password[i] = charGroups[nextGroupIdx][nextCharIdx];
            
				if (lastCharIdx == 0)
					charsLeftInGroup[nextGroupIdx] = 
												charGroups[nextGroupIdx].Length;
				else
				{
					if (lastCharIdx != nextCharIdx)
					{
						char temp = charGroups[nextGroupIdx][lastCharIdx];
						charGroups[nextGroupIdx][lastCharIdx] = 
									charGroups[nextGroupIdx][nextCharIdx];
						charGroups[nextGroupIdx][nextCharIdx] = temp;
					}
					charsLeftInGroup[nextGroupIdx]--;
				}

				if (lastLeftGroupsOrderIdx == 0)
					lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
				else
				{
					if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
					{
						int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
						leftGroupsOrder[lastLeftGroupsOrderIdx] = 
									leftGroupsOrder[nextLeftGroupsOrderIdx];
						leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
					}
					lastLeftGroupsOrderIdx--;
				}
			}

			return new string(password);	 
		}

		public void CleanProvider(string providerUserId)
		{
			_repository.CleanProvider(providerUserId);
		}

		public List<User> GetAll()
		{
			return _repository.GetAll();
		}
    }
}
