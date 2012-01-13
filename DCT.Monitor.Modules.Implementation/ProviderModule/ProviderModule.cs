using DCT.Monitor.Entities;
using DCT.Monitor.Modules;
using Monitor.DAL;
using DCT.Unity;

namespace DCT.Monitor.Modules.Implementation.ProviderModule
{
	public class ProviderModule : IProviderModule
	{
		private static IProviderRepository _repository = ServiceLocator.Current.Resolve<IProviderRepository>();

		public ProviderModule()
		{
		}

		public void Create(/*Provider provider*/)
		{
			//_repository.Create(/*provider*/);
		}

		public Provider GetProvider(string providerName)
		{
			return _repository.GetProvider(providerName);
		}

		public Provider GetProvider(int providerId)
		{
			return _repository.GetById(providerId);
		}
	}
}
