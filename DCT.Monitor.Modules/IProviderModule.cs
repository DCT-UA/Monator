using DCT.Monitor.Entities;

namespace DCT.Monitor.Modules
{
	public interface IProviderModule
	{
		void Create();
		Provider GetProvider(string providerName);
		Provider GetProvider(int providerId);
	}
}
