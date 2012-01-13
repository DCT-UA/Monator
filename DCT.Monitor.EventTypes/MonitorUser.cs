using System;

namespace DCT.Monitor.Entities
{
	[Serializable]
    public class User: IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
		public int ProviderId { get; set; }
		public string ProviderUserId { get; set; }
        public string Role { get; set; }

        public bool IsAdministrator { get { return (Role ?? "").Contains("a"); } }
    }

	public class Provider : IEntity<int>
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
